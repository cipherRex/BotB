
var jsInterop = jsInterop || {};
var _selectedFighterId = '';
var _selectedPicture = 'k1.png';
var _playerFighters;

var unityInstance;
//OpeningSequenceComplete

var _interval;
var _size = 0;

function onFighterSelected(data)
{
    _selectedFighterId = data.selectedData.value;

    $("#enterButton").removeClass("hiddenButton");
}
 
function onPictureSelected(data) {
    _selectedPicture = data.selectedData.value;
}

//jsInterop.initializeFightersDropDown = function (jsonString) {

//    _playerFighters = JSON.parse(jsonString);

//    $('#playerFighterSelect').ddslick({
//        data: _playerFighters,
//        selectText: "Select your warrior",
//        imagePosition: "right",
//        width: 290,
//        onSelected: function (data) {
//            onFighterSelected(data);
//        }
//    });
//};

window.botBFunctions = {

    countdownComplete: function () {

        DotNet.invokeMethodAsync('BotB.Client', 'onCountdownComplete');
    },

    getSelectedPictureFilename: function () {
        return _selectedPicture;
    },

    getSelectedFighterId: function () {
        return _selectedFighterId;
    },

    startGame: function () {
        unityInstance = UnityLoader.instantiate("unityContainer", "webGl/Build/Build.json", { onProgress: UnityProgress });
    },

    initializeFightersDropDown: function (jsonString) {
        _playerFighters = JSON.parse(jsonString);

        $('#playerFighterSelect').ddslick({
            data: _playerFighters,
            selectText: "Select your warrior",
            imagePosition: "right",
            width: 290,
            onSelected: function (data) {
                onFighterSelected(data);
            }
        });;
    },

    initCountdown: function () {
        _size = 0;
        _interval = setInterval(
            function () {
                
                _size = _size + 10;
                $('#myBar').css('width', _size + '%');


                if (_size >= 100) {
                    _size = 0;
                    clearInterval(_interval);
                    botBFunctions.countdownComplete();

                }

            }, 1000
        );

    },

    cancelCountdown: function () {
        _size = 0;
        clearInterval(_interval);
        $('#myBar').css('width', _size + '%');
    },


    initializeFighterPicturesDropDown: function (x) {

        let pictures = [
            {
                "text": "",
                "value": "k1.png",
                "selected": true,
                "description": "",
                "imageSrc": "/images/k1.png"
            },
            {
                "text": "",
                "value": "k2.png",
                "selected": false,
                "description": "",
                "imageSrc": "/images/k2.png"
            },
            {
                "text": "",
                "value": "k3.png",
                "selected": false,
                "description": "",
                "imageSrc": "/images/k3.png"
            },
            {
                "text": "",
                "value": "k4.png",
                "selected": false,
                "description": "",
                "imageSrc": "/images/k4.png"
            }
        ]

        $('#pictureSelect').ddslick({
            data: pictures,
            selectText: "Select picture",
            imagePosition: "right",
            width: 100,
            onSelected: function (data) {
                onPictureSelected(data);
            }
        });
    } ,

    whiteSwing: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteSwing");
    },

    whiteBlock: function (serializedBoolArray) {
        unityInstance.SendMessage("JavascriptHook", "WhiteBlock", serializedBoolArray);
    },

    whiteParry: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteParry");
    },

    whiteCounterParry: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteCounterParry");
    },

    whiteHeal: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteHeal");
    },

    whiteGashed: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteGashed");
    },

    whiteGroined: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteGroined");
    },

    whiteKick: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteKick");
    },

    whiteTwoHanded: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteTwoHanded");
    },

    whiteCelebrate: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteCelebrate");
    },

    whiteLaugh: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteLaugh");
    },

    whiteRun: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteRun");
    },

    whiteDie: function () {
        unityInstance.SendMessage("JavascriptHook", "WhiteDie");
    },

    blackSwing: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackSwing");
    },

    blackBlock: function (serializedBoolArray) {
        unityInstance.SendMessage("JavascriptHook", "BlackBlock", serializedBoolArray);
    },

    blackParry: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackParry");
    },

    blackCounterParry: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackCounterParry");
    },

    blackHeal: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackHeal");
    },

    blackGashed: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackGashed");
    },

    blackGroined: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackGroined");
    },

    blackKick: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackKick");
    },

    blackTwoHanded: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackTwoHanded");
    },

    blackCelebrate: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackCelebrate");
    },

    blackLaugh: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackLaugh");
    },

    blackRun: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackRun");
    },

    blackDie: function () {
        unityInstance.SendMessage("JavascriptHook", "BlackDie");
    },

    whiteHealEffect: function (amt) {
        unityInstance.SendMessage("JavascriptHook", "WhiteHealEffect", amt);
    },

    whiteBleedEffect: function (amt) {
        unityInstance.SendMessage("JavascriptHook", "WhiteBleedEffect", amt);
    },

    blackHealEffect: function (amt) {
        unityInstance.SendMessage("JavascriptHook", "BlackHealEffect", amt);
    },

    blackBleedEffect: function (amt) {
        unityInstance.SendMessage("JavascriptHook", "BlackBleedEffect", amt);
    },



    whiteSetHPs: function (amt) {
        unityInstance.SendMessage("JavascriptHook", "SetWhiteHPs", amt);
    },

    blackSetHPs: function (amt) {
        unityInstance.SendMessage("JavascriptHook", "SetBlackHPs", amt);
    },

    whiteSetDmg: function (amt) {
        unityInstance.SendMessage("JavascriptHook", "setWhiteDmg", amt);
    },

    blackSetDmg: function (amt) {
        unityInstance.SendMessage("JavascriptHook", "setBlackDmg", amt);
    },

    setSystemMsg: function (msg) {
        unityInstance.SendMessage("JavascriptHook", "setSystemMsg", msg);
    },

    hideHPLabels: function () {
        unityInstance.SendMessage("JavascriptHook", "hideHPLabels");
    },

    rotateCamera: function () {
        unityInstance.SendMessage("JavascriptHook", "endGame");
    },

    playVictoryCheers: function () {
        unityInstance.SendMessage("JavascriptHook", "PlayVictoryCheers");
    }
};

