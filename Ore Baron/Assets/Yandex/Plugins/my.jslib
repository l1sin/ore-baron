mergeInto(LibraryManager.library, {

  GetLanguage: function () {
    var lang = ysdk.environment.i18n.lang;

    var bufferSize = lengthBytesUTF8(lang) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(lang, buffer, bufferSize);
    return buffer;
  },

  DebugJS: function (message) {
    console.log(UTF8ToString(message));
  },

  Rate: function () {
    ysdk.feedback.canReview()
    .then(({ value, reason }) => {
      if (value) {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) => {
          console.log(feedbackSent);
        })
      } else {
        console.log(reason)
      }
    })
  },

  WatchAdMine: function () {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('GameController', 'ToggleMineAdBonus', 1);
          console.log('Rewarded: Mine');
        },
        onClose: () => {
          console.log('Video ad closed.');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  WatchAdClick: function () {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
        },
        onRewarded: () => {
          myGameInstance.SendMessage('GameController', 'ToggleClickAdBonus', 1);
          console.log('Rewarded: Click');
        },
        onClose: () => {
          console.log('Video ad closed.');
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  BuyMine: function () {

  },

  BuyClick: function () {

  },

  SaveExtern: function (data) {
    var dataString = UTF8ToString(data);
    var myobj = JSON.parse(dataString);
    player.setData(myobj, true);
    console.log("SaveExtern");
  },

  LoadExtern: function () {
    player.getData().then(_data => {
      const myJSON = JSON.stringify(_data);
      myGameInstance.SendMessage('Yandex', 'ApplySave', myJSON);
    });
  },

  FullScreenAd: function () {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          // some action after close
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

});