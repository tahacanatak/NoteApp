var apiUrl = "http://localhost:55045/"

var app = angular.module("myApp", ["ngRoute"]);

app.controller("indexCtrl", function ($scope, $window, $http, $location) {

    $scope.isAuthenticated = false;

    $scope.token = function () {
        if ($window.sessionStorage.token) {
            return $window.sessionStorage.token;
        }
        if ($window.localStorage.token) {
            return $window.localStorage.token;
        }
        return null;
    };

    $scope.requestConfig = function () {
        return {
            headers: {
                Authorization: "Bearer " + $scope.token()
            }
        }
    };

    $scope.checkAuthentication = function () {

        //giris yapıldı mı?
        if ($scope.token() == null) {
            $scope.isAuthenticated = false;
            $scope.loggedInUserEmail = null;
            return false;
        }

        $http.get(apiUrl + "api/Account/UserInfo", $scope.requestConfig()).then(
            function (response) {
                $scope.loggedInUserEmail = response.data.Email;
                $scope.isAuthenticated = true;
            },
            function (response) {
                $scope.isAuthenticated = false;
                $scope.loggedInUserEmail = null;
            }
        );
    };

    $scope.setLoggedInUser = function (email) {
        if (email) {
            $scope.isAuthenticated = true;
            $scope.loggedInUserEmail = email;
        }
        else {
            $scope.isAuthenticated = false;
            $scope.loggedInUserEmail = null;
        }
    };

    $scope.logout = function (e) {
        e.preventDefault();

        $scope.setLoggedInUser(null);
        $http.post(apiUrl + "api/Account/Logout", null, $scope.requestConfig()).then(
            function (response) {

            }
        );

        $window.sessionStorage.removeItem("token");
        $window.localStorage.removeItem("token");
        $location.path("login");
    };

    $scope.checkAuthentication();
});

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "Pages/Main.html",
            controller: "mainCtrl"
        })
        .when("/login", {
            templateUrl: "Pages/Login.html",
            controller: "loginCtrl"
        })
        .when("/register", {
            templateUrl: "Pages/Register.html",
            controller: "registerCtrl"
        })
});

app.controller("mainCtrl", function ($scope, $http, $window, $location) {
    if (!$scope.token()) { // == null
        $location.path("login");
        return;
    }

   
    $scope.isLoading = true;
    $scope.notes = []; // notlar bu dizide tutulacak


    $scope.loadNotes = function () {
        $http.get(apiUrl + "api/Notes/GetNotes", $scope.requestConfig()).then(
            function (response) {
                $scope.notes = response.data;
                $scope.isLoading = false;
            },
            function (response) {
                if (response.status == 401) {
                    $location.path("login");
                }
            }
        );
    };

    $scope.loadNotes();


});


app.controller("loginCtrl", function ($scope, $http, $httpParamSerializer, $window, $location, $timeout) {
    $scope.errors = [];
    $scope.message = "";


    //default user bilgileri
    $scope.user = {
        grant_type: "password",
        username: "tahacan.atak@gmail.com",
        password: "Ankara1."
    };
    //beni hatirla btnu default 
    $scope.isRememberMe = false;

    //hatayi login.htmle gönderen fonk.
    $scope.hasErrors = function () {
        return $scope.errors.length > 0;
    };


    $scope.login = function (e) {
        e.preventDefault();

        //butona her bastiginda alertleri bosalt
        $scope.errors = [];
        $scope.successMessage = "";

        $http.post(apiUrl + "Token", $httpParamSerializer($scope.user)).then(
            function (response) { //basarı durumu
                var token = response.data.access_token; //token alındı

                //varsa önce mevcut tokenları temizle
                if ($window.localStorage.token)
                    $window.localStorage.removeItem("token");
                if ($window.sessionStorage.token)
                    $window.sessionStorage.removeItem("token");


                if ($scope.isRememberMe) {
                    $window.localStorage.token = token;
                }
                else {
                    $window.sessionStorage.token = token;
                }

                $scope.setLoggedInUser($scope.user.username);

                $scope.successMessage = "Başarıyla giriş yaptınız. Şimdi uygulamaya yönlendiriliyorsunuz..";

                $timeout(function () {
                    $location.path("/");
                }, 1000);
            },
            function (response) { //hata durumu
                if (response.data.error_description) {
                    $scope.errors.push(response.data.error_description);
                }

            }
        )
    }

});


app.controller("registerCtrl", function ($scope, $http) {
    $scope.errors = [];
    $scope.message = "";

    $scope.user = {
        Email: "tahacan.atak@gmail.com",
        Password: "Ankara1.",
        ConfirmPassword: "Ankara1."
    };

    $scope.register = function (e) {
        $scope.errors = [];
        e.preventDefault();
        $http.post(apiUrl + "api/Account/Register", $scope.user)
            .then(function (response) {
                $scope.user = { Email: "", Password: "", ConfirmPassword: "" };
                $scope.successMessage = "Kayıt başarılı. Şimdi giriş sayfasından giriş yapabilirsiniz.";
            }, function (response) {
                $scope.errors = getErrors(response.data.ModelState);
            });
    };

    //hatayi register.htmle gönderen fonk.
    $scope.hasErrors = function () {
        return $scope.errors.length > 0;
    };

});

// hatali istek yapıldıgında gelen hataları donduren fonk.
function getErrors(modelState) {

    var errors = [];

    for (var key in modelState) {
        for (var i = 0; i < modelState[key].length; i++) {
            errors.push(modelState[key][i]);

            // ayni hatayi bastiği durum icin
            if (modelState[key][i].includes("zaten alınmış")) {
                break;
            }
        }
    }

    return errors;
}
