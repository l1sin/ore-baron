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

  },

  WatchAdClick: function () {

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

});