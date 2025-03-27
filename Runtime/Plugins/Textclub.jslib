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

    JS_scheduleNotification: function (options) {
        let opts = JSON.parse(UTF8ToString(options));

        opts.scheduledAt = new Date(opts.scheduledAt);

        window.textclubSdk.scheduleNotification(opts);
    },

    JS_captureEvent: function (eventName, properties) {
        window.textclubSdk.captureEvent(UTF8ToString(eventName), JSON.parse(UTF8ToString(properties)));
    },

    JS_initSdk: function (taskPtr, successCallback, errorCallback) {
        window.textclubSdk.isReady().then(function () {
            {{{ makeDynCall('vi', 'successCallback') }}} (taskPtr);
        })
            .catch(function (error) {
                {{{ makeDynCall('vii', 'errorCallback') }}} (taskPtr, marshalString(error));
            });
    },

    JS_callAsyncVoid: function (taskPtr, callName, successCallback, errorCallback) {
        window.textclubSdk[UTF8ToString(callName)].then(function () {
            { { { makeDynCall('vi', 'successCallback') } } } (taskPtr);
        })
            .catch(function (error) {
                { { { makeDynCall('vii', 'errorCallback') } } } (taskPtr, marshalString(error));
            });
    },

    JS_callAsyncNumber: function (taskPtr, callName, successCallback, errorCallback) {
        window.textclubSdk[UTF8ToString(callName)].then(function (result) {
            { { { makeDynCall('vif', 'successCallback') } } } (taskPtr, result);
        })
            .catch(function (error) {
                { { { makeDynCall('vii', 'errorCallback') } } } (taskPtr, marshalString(error));
            });
    },

    JS_callAsyncString: function (taskPtr, callName, successCallback, errorCallback) {
        window.textclubSdk[UTF8ToString(callName)].then(function (result) {
            { { { makeDynCall('vii', 'successCallback') } } } (taskPtr, result);
        })
            .catch(function (error) {
                { { { makeDynCall('vii', 'errorCallback') } } } (taskPtr, marshalString(error));
            });
    }
});
