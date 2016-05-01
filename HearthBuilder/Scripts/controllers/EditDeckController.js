var myapp = angular.module('HearthBuilder', ['ngSanitize', 'ui.bootstrap', 'ngAnimate'])
.controller("EditDeckController", ["$scope", "$http", "$compile", "$timeout", '$uibModal', '$window', function ($scope, $http, $compile, $timeout, $uibModal, $window) {

    // in controller

    $scope.txtSearch = "";
    $scope.allCards = [];
    $scope.filteredCards = [];
    $scope.deckCards = [];
    $scope.animationsEnabled = true;

    //called on page load
    $scope.init = function ($deckName, $deckId, $playerClass) {
        $scope.txtDeckName = $deckName;
        $scope.deckId = $deckId;
        $scope.playerClass = $playerClass;


        var responsePromise = $http.get("/Deck/ListAllCards/");
        responsePromise.success(function (data, status, headers, config) {
            $scope.allCards = data;
            $scope.filterCards(); //do a fresh listing of filtered cards
            $scope.refreshDeck(); //refresh the player's deck
        });
        responsePromise.error(function (data, status, headers, config) {
            $scope.showNotification("Error! init",  data.Message, "alert-danger");
        });
    };

    $scope.addCardClick = function(item) {
        $scope.showNotification("Adding card...", "", "alert-warning");
        var cardId = item.currentTarget.getAttribute("data-id");
        var responsePromise = $http.post("/Deck/AddCard/", { id : cardId });
        responsePromise.success(function(data, status, headers, config) {
            angular.forEach(data, function (result){
                if (result.Result == '1'){
                    $scope.showNotification("Success!", "Card Added.", "alert-success");
                    $scope.refreshDeck();
                }
                else {
                    $scope.showNotification("Error!", result.Message, "alert-danger");
                }
            });
        });
        responsePromise.error(function(data, status, headers, config) {
            $scope.showNotification("Error! init",  data.Message, "alert-danger");
        });
    }

    $scope.delCardClick = function(item) {
        $scope.showNotification("Deleting card...", "", "alert-warning");
        var cardId = item.currentTarget.getAttribute("data-id");
        var responsePromise = $http.post("/Deck/RemoveCard/", { id : cardId });
        responsePromise.success(function(data, status, headers, config) {
            angular.forEach(data, function (result){
                if (result.Result == '1'){
                    //we added a card, lets update the Deck List
                    $scope.showNotification("Success!", "Card Removed.", "alert-success");
                    $scope.refreshDeck();
                }
                else {
                    $scope.showNotification("Error!", "Card couldn't be removed!? " + result.Message, "alert-danger");
                }
            });
        });
        responsePromise.error(function(data, status, headers, config) {
            $scope.showNotification("Error! init",  data.Message, "alert-danger");
        });
    }

    $scope.saveDeck = function() {
        $scope.showNotification("Saving to DB...", "", "alert-warning");
        var responsePromise = $http.post("/Deck/SaveDeck/", { id : $scope.txtDeckName });
        responsePromise.success(function(data, status, headers, config) {
            angular.forEach(data, function (result){
                if (result.Result == '1'){
                    $scope.showNotification("Success!", result.Message, "alert-success");
                    //we saved the deck successfully
                    $scope.refreshDeck();
                    if (result.ShouldRedirect === true)
                        $window.location.href = '/Deck/Edit/' + result.NewId;
                }
                else {
                    $scope.showNotification("Error!", "Couldn't save deck!? " + result.Message, "alert-danger");
                }
            });

        });
        responsePromise.error(function(data, status, headers, config) {
            $scope.showNotification("Error! init",  data.Message, "alert-danger");
        });
    }

    $scope.saveDeckClick = function() {
        if ($scope.cardCount < 30) { //is this deck not finished?
            var modalInstance = $uibModal.open({
                templateUrl: '/Content/ModalConfirm.html',
                size: 'sm',
                scope: $scope,
                resolve: {
                    message: function () {
                        return "This deck is not yet finished (requires a total of 30 cards). Would you like to save anyways?";
                    },
                    title: function () {
                        return "Save Deck";
                    }
                },
                controller:function($uibModalInstance, $scope, message, title){
                    $scope.promptMessage = message;
                    $scope.promptTitle = title;
                    $scope.ok = function () {
                        $scope.saveDeck();
                        $uibModalInstance.dismiss('cancel');
                    }
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                },
            });
        } else { //it is, save it
            $scope.saveDeck();
        }
    }

    $scope.cardPopover = {
        templateUrl: '/Content/templates/TemplateCardPopover.html',
        cardId: ''
    }

    $scope.cardHover = function(item) {
        $scope.cardPopover.cardUrl = "/Content/Images/cards/"+item.currentTarget.getAttribute("data-id")+".png";
    }

    $scope.cardCount = 0;
    $scope.refreshDeck = function () {
        $scope.deckCards = [];
        var responsePromise = $http.get("/Deck/SessionDeck/");
        responsePromise.success(function(data, status, headers, config) {
            var count = 0;
            angular.forEach(data.Cards, function (card) {
                //set the defaults for the card
                card.hasDuplicate = false;
                card.isDuplicate = false;

                //we don't want to show duplicate cards, instead we should show a counter beside the card
                //so we loop through again, looking for another occurrance
                var tmpCount = 0;
                angular.forEach(data.Cards, function (tempCard) {
                    if (card.Id == tempCard.Id && count != tmpCount) { //if we have duplicate cards
                        if (count < tmpCount) { //if were on the first occurance of the card
                            card.hasDuplicate = true;
                        } else if (count > tmpCount) {  //if were on the 2nd occurance of the card
                            card.isDuplicate = true;
                        }
                    }
                    tmpCount += 1;
                });
                $scope.deckCards.push(card); //add it to the deck list
                count += 1;
            });
            $scope.cardCount = count; //update the counter on the page
        });
        responsePromise.error(function(data, status, headers, config) {
            alert("AJAX failed! refresh" + data.Message);
        });
    }

    $scope.filterCards = function () {

        $scope.filteredCards = [];
        var results = "";
        angular.forEach($scope.allCards, function (card) { //loop through every card
            if (card.Class == $scope.playerClass || card.Class == 0) { //filter by current class, and non-class
                if (card.Name.toLowerCase().indexOf($scope.txtSearch.toLowerCase()) > -1 || (card.Text != null && card.Text.toLowerCase().indexOf($scope.txtSearch.toLowerCase()) > -1)) { //filter by textbox
                    $scope.filteredCards.push(card);
                }
            }
        });
    }

    $scope.showNotificationTimeout = {};
    $scope.notification = {
        title: "",
        message: "",
        type: "",
    }
    $scope.showNotification = function (title, msg, type) {

        $scope.notification.title = title;
        $scope.notification.message = msg;
        $scope.notification.type = type;
        $scope.showNotificationMessage = true; //show it

        $timeout.cancel($scope.showNotificationTimeout); //cancel the previous timeout, weve got a new notification to show
        $scope.showNotificationTimeout = $timeout(function () { //hide it after 5s
            $scope.showNotificationMessage = false;
        }, 5000);
        
    }

    $scope.delDeckClick = function () {
        var modalInstance = $uibModal.open({
            templateUrl: '/Content/ModalConfirm.html',
            size: 'sm',
            scope: $scope,
            resolve: {
                message: function () {
                    return "Are you sure you want to delete this deck? This cannot be undone!";
                },
                title: function () {
                    return "Delete Deck";
                }
            },
            controller:function($uibModalInstance, $scope, message, title){
                $scope.promptMessage = message;
                $scope.promptTitle = title;
                $scope.ok = function () {
                    console.log("Deleting deck...");
                    $scope.showNotification("Deleting deck...", "", "alert-warning");
                    $uibModalInstance.dismiss('cancel');
                    var responsePromise = $http.post("/Deck/DeleteDeck/");
                    responsePromise.success(function(data, status, headers, config) {
                        angular.forEach(data, function (result){
                            if (result.Result == '1'){
                                console.log("Deck Removed.");
                                $scope.showNotification("Success!", "Deck Removed.", "alert-success");
                                $window.location.href = '/';
                            }
                            else {
                                console.log("Deck couldn't be removed!? "  + result.Message);
                                $scope.showNotification("Error!", "Deck couldn't be removed!? " + result.Message, "alert-danger");
                            }
                        });
                    });
                }
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
            },
        });

        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
    };

}])