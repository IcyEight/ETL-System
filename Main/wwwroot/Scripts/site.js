function showPopup(popupType, popupTitle, popupMessage) {
    BootstrapDialog.show({
        type: popupType,
        title: popupTitle,
        message: popupMessage,
        buttons: [{
            label: 'OK',
            action: function (dialog) {
                dialog.close();
            }
        }]
    }); 
}