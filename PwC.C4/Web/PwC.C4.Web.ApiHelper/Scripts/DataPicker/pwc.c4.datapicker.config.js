var serviceSettings =
{
    "people": {
        template: "people",
        cache_setting: {
            enable: true,
            expiration:3000,//秒
            cacheMethod: sessionStorage
        },
        ajax_setting: {
            method_search: "post",
            url_search: "http://cnpekappdwv031/PwC.C4.ApiHelper/api/values/search",
            method_fetch: "post",
            url_fetch: "http://cnpekappdwv031/PwC.C4.ApiHelper/api/values/fetch",
            method_pic: "post",
            url_pic: "http://cnpekappdwv031/PwC.C4.ApiHelper/api/values/pic"
        },
        display_item_setting: {
            key: "StaffId",
            value: "StaffName",
            icon_setting: "width=45&height=45&quality=95&format=jpg&mode=crop",
            properties: "StaffName,StaffId,Type",
            template: "displayItem.html"
        },
        autocomplete_setting: {
            key: "StaffId",
            value: "StaffName",
            properties: "StaffName,StaffId,DivName,Type,Count",
            queryProps: "StaffName,StaffId,Email",
            highlight: {
                enable: true,
                displayPropertiy:"StaffName"
            },
            template: "autocomplete.html",
            emptyAlert: "<li class='dpac-li'>No staff data</li>",
            errorAlert: "<li class='dpac-li'>System error!</li>",
            loadingAlert: "<li class='dpac-li'><span class=\"bootstrap-dialog-button-icon glyphicon glyphicon-asterisk icon-spin\"></span>Loading!</li>",
            keyUpDelayTime: 180
        },
        search_dialog_setting: {
            title: "People picker",
            template: "searchDialog.html",
            postion: "middle",
            lazyloading: true,
            closable: false,
            draggable: false,
            closeByBackdrop: false,
            closeByKeyboard: false,
            isShowDefaultList: true,
            enablePic: true,
            key: "StaffId",
            emptyAlert: "<li style=\"text-align: center;font-size: 20px;padding-top: 90px;\">No data</li>",
            errorAlert: "<li style=\"text-align: center;font-size: 20px;padding-top: 90px;\">System Error!</li>",
            icon_setting: "width=65&height=65&quality=95&format=jpg&mode=crop",
            properties: "StaffName,StaffId,LoSDesc,JobTitle,PhoneNo,City,DivName,Type,Count",
            search_item: {
                queryProps: "StaffName,StaffId,Email",
                template: "searchItem.html",
                key: "StaffId",
                value: "StaffName"
            }
        }
    }

}

