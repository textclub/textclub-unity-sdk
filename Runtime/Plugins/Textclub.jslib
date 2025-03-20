mergeInto(LibraryManager.library, {

    JS_getPlayerId: function () {
        const val = window.textclubSdk.getPlayer().playerId;
        console.log(val);
        return marshalString(val);
    },

    JS_getIsRegistered: function () {
        const val = window.textclubSdk.getPlayer().registered ? "true" : "false";
        return marshalString(val);
    },

    JS_getPlayerValue: function (key) {
        const val = window.textclubSdk.getPlayerDataVal(UTF8ToString(key));
        return marshalString(val);
    },

    JS_setPlayerValue: function (key, value) {
        window.textclubSdk.setPlayerDataVal(UTF8ToString(key), UTF8ToString(value))
    },

});
