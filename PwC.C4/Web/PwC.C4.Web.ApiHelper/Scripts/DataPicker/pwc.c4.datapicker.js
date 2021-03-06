﻿(function ($) {

    var _globalSettings = [];
    var _service = null;
    var _template = null;
    var _cacheEnable = false;
    var _cacheMethod = sessionStorage;
    var _dialogCurrentMode = [];
    var _dialogCurrentValues = [];
    var _dialogHasShownAllList = [];
    var _dialogLoadMore = [];
    var _dialogHasShownAll = [];
    var _autoCompleteLock = [];
    var _cacheDisplayKey = "dp-dis-";
    var _cacheDialogKey = "dp-dia-";
    var _cachePicKey = "dp-pic-";
    var _cacheAutoKey = "dp-auto-";
    var _cacheTempKey = "dp-temp-";
    var _dialog = [];
    var chars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];

    //Base function 

    var _dpCache = function () {
        if (_cacheEnable) {
            var cacheTime = _cacheMethod.getItem("dp-expiration");
            var timeSpin = new Date().getTime();
            if (cacheTime === null || cacheTime === "") {
                _cacheMethod.setItem("dp-expiration", JSON.stringify(timeSpin));
            } else {
                var x = (parseInt(timeSpin) - parseInt(cacheTime)) > (_service.cache_setting.expiration * 1000);
                if (x) {
                    var i = _cacheMethod.length;
                    while (i--) {
                        var ki = sessionStorage.key(i);
                        if (/^dp-/.test(ki)) {
                            sessionStorage.removeItem(ki);
                        }
                    }
                }
            }
            return _cacheMethod;
        } else {
            var ii = _cacheMethod.length;
            while (ii--) {
                var kii = sessionStorage.key(i);
                if (/^dp-/.test(kii)) {
                    sessionStorage.removeItem(kii);
                }
            }
        }
        return null;
    }

    var _dpCacheKey = function(method) {
        if (method === "display") {
            return  _cacheDisplayKey;
        }
        else if (method === "auto") {
            return _cacheAutoKey;
        } else if (method === "dialog") {
            return _cacheDialogKey;
        } else if (method === "pic") {
            return _cachePicKey;
        }else{
        return _cacheTempKey;
        }
    }

    var _dpGetDataKey = function(method, obj) {
        if (method === "display") {
           if (obj.hasOwnProperty(_service.display_item_setting.key)) {
               return obj[_service.display_item_setting.key];
           }
            return null;
        }
        else if (method === "auto") {
            if (obj.hasOwnProperty(_service.autocomplete_setting.key)) {
                return obj[_service.autocomplete_setting.key];
            }
            return null;
        } else if (method === "dialog") {
            if (obj.hasOwnProperty(_service.search_dialog_setting.search_item.key)) {
                return obj[_service.search_dialog_setting.search_item.key];
            }
            return null;
        } else if (method === "pic") {
            if (obj.hasOwnProperty(_service.search_dialog_setting.search_item.key)) {
                return obj[_service.search_dialog_setting.search_item.key];
            }
            return null;
        } else {
            if (obj.hasOwnProperty("tempkey")) {
                return obj["tempkey"];
            }
        }
    }

    var _dpGetCacheDatas = function(method, keys) {
        if (_dpCache() == null) {
            return null;
        }
        var fullCache = true;
        var values = [];
        var k = _dpCacheKey(method);
        
        $.each(keys, function (i, v) {
            var va = _cacheMethod.getItem(k + v);
            if (va !== null && va !== undefined && va !== "" && va !== "[object Object]") {
                values.push(JSON.parse(va));
            } else {
                fullCache = false;
                values.push("EmptyCache:" + v);
            }
        });
        var returnData = { Data: values, IsFullCache: fullCache };
        return returnData;
    }

    var _dpSetCacheDatas = function (method, values) {
        if (_dpCache() == null) {
            return;
        }
        var k = _dpCacheKey(method);
        $.each(values, function (i, v) {            
            var key = _dpGetDataKey(method, v);
            var va = JSON.stringify(v);
            _cacheMethod.setItem(k + key, va);
        });
    }

    function _dpAjax(settings,func, datas, callback) {
        var self = this;
        var url, method, as = _service.ajax_setting;
        var urlPara = [];
        for (var propertyName in settings.para) {
            if (settings.para.hasOwnProperty(propertyName)) {
                urlPara.push(propertyName + "=" + settings.para[propertyName]);
            }
        }
        if (func === "fetch") {
            url = as.url_fetch;
            method = as.method_fetch;
        } else if (func === "search") {
            url = as.url_search;
            method = as.method_search;
        } else {
            url = as.url_pic;
            method = as.method_pic;
        }
        if (urlPara.length > 0) {
            url = url + "?" + urlPara.join("&");
        }
        
        $.ajax({
            url: url, type: method, data: datas, datatype: "jsonp", success: function (data, textStatus, jqXhr) {
                callback(data, textStatus, jqXhr);
            }
        });
    }

    var _dpDynamicLoading = {
        css: function (path) {
            if (!path || path.length === 0) {
                throw new Error('argument "path" is required !');
            }
            var head = document.getElementsByTagName('head')[0];
            var link = document.createElement('link');
            link.href = path;
            link.rel = 'stylesheet';
            link.type = 'text/css';
            head.appendChild(link);
        },
        js: function (path) {
            if (!path || path.length === 0) {
                throw new Error('argument "path" is required !');
            }
            var head = document.getElementsByTagName('head')[0];
            var script = document.createElement('script');
            script.src = path;
            script.type = 'text/javascript';
            head.appendChild(script);
        }
    }

    var _dpScriptSource = (function () {
        var p;
        var scripts = document.getElementsByTagName('script'),
            script = scripts[scripts.length - 1];
        p = script.getAttribute('src', 2);
        return p.substring(0, p.lastIndexOf("/"));
    }());

    var _dpDelay = (function () {
        var timer = 0;
        return function (callback, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();

    var _dpRegExpEscape = function (s) {
        return s.replace(/[-\\^$*+?.()|[\]{}]/g, "\\$&");
    };

    var _dpBulidBaseDiv = function(settings, el, numId) {
        var self = this;
        var dtd = $.Deferred();
        $(el).css("display", "none");
        var itemId = "dataPick-" + numId;
        var area = $("<div>")
            .addClass("dpWrapper")
            .css("width", settings.width)
            .css("height", settings.height)
            .css("margin", "auto auto")
            .css("clear", "both");
        area.append(settings.desc);
        var box = $("<div>").attr("id", itemId).attr("dpid", numId).addClass("dpBoxWrapper");
        if (settings.autocomplete.enable) {
            var inputbox = $("<div id='con-dpBox" + numId + "'>").addClass("dpComplete");
            if (settings.readOnly) {
                inputbox.addClass("dp-displayNone");
            }
            inputbox.append("<input type='text' id='in-dpBox" +
                numId +
                "' class='dpBox' autocomplete='off' aria-autocomplete='list'/>");
            var sugg = $("<ul>");
            var span =
                "<span class='visually-hidden' role='status' aria-live='assertive' aria-relevant='additions'></span>";
            inputbox.append(sugg);
            inputbox.append(span);
            box.append(inputbox);
        }
        area.append(box);
        var btnClass = "dpBtnReadOnly";
        if (!settings.readOnly) {
            btnClass = "dpBtn";
        }
        area.append("<div id='di-dpBtn" + numId + "' dpid='" + numId + "' class='" + btnClass + "'></div>");
        $(el).before(area.html());
        $(el).attr("dpid", numId);
        var value = $(el).val();
        if (value != undefined && value !== "") {
            _dpRenderDisplayItem(numId,
                value,
                function() {
                    dtd.resolve();
                    return dtd.promise(numId, value);
                });

        } else {
            _dpResetInput(numId);
            dtd.resolve();
            return dtd.promise(numId, value);
        }

    };

    function _dpArrayUnique(arr) {
        var result = [], hash = {};
        for (var i = 0, elem; (elem = arr[i]) != null; i++) {
            if (!hash[elem]) {
                result.push(elem);
                hash[elem] = true;
            }
        }
        return result;
    }

    //Render or template
    
    function _dpRenderTemplate(templateFileName, data, callback) {
        _dpCache();
        var te = _cacheMethod.getItem(_cacheTempKey + templateFileName);
        if (te != null && te !== "") {
            var tempFn = doT.template(te);
            var html = tempFn(data);
            callback(html);
        } else {
            _dpGetTemplateFile(templateFileName, function (temp) {
                _cacheMethod.setItem(_cacheTempKey + templateFileName, temp);
                var tempFn = doT.template(temp);
                var html = tempFn(data);
                callback(html);
            });
        }
    }

    function _dpGetTemplateFile(fileName, callback) {
        var filePath = _dpScriptSource + "/template/" + _template + "/" + fileName;
        $.ajax({
            type: "GET",
            url: filePath,
            datatype: "text",
            success: function (html) {
                callback(html);
            }
        });
    }

    function _dpRenderDisplayItem(numId, value, callback) {
        var self = this;
        var itemId = "dataPick-" + numId;
        if (value === "") {
            _dpRenderDisplayHtml(itemId, numId, "");
            if (callback != null)
                callback(numId, value);
            return;
        }
        
        var disItem = _service.display_item_setting;
        var disProperties = disItem.properties.split(",");
        var d = _dpGetCacheDatas("display", value.split(_globalSettings[numId].separator));
        
        if (d !== null && d !== undefined && d.IsFullCache) {
            d.Data.readOnly = _globalSettings[numId].readOnly;
            var valFot = { data: d.Data, readOnly: _globalSettings[numId].readOnly };
            _dpRenderTemplate(disItem.template, valFot, function (html) {
                _dpRenderDisplayHtml(itemId, numId, html);
                if (callback != null && callback != undefined)
                    callback(numId, value);
            });
        } else {
            _dpAjax(_globalSettings[numId],"fetch", { k: value, s: _globalSettings[numId].separator, p: disProperties, ico: disItem.icon_setting }, function (resp) {
                var data = _dpFixSelectedStatus(numId, resp.Data);
                _dpSetCacheDatas("display", data);
                var valFot = { data: data, readOnly: _globalSettings[numId].readOnly };
                _dpRenderTemplate(disItem.template, valFot, function (html) {
                    _dpRenderDisplayHtml(itemId, numId, html);
                    if (callback != null && callback != undefined)
                        callback(numId, value);
                });
            });
        }
    }

    function _dpRenderDisplayHtml(itemId, numId, html) {
        var self = this;
        if (_globalSettings[numId].autocomplete.enable) {

	        var oldDisItem = $("#" + itemId).find(".dpac-selected");
	        if (oldDisItem.length > 0) {
	            oldDisItem.remove();
	        }
	        var oldDisItemDis = $("#" + itemId).find(".dpac-selectedReadOnly");
	        if (oldDisItemDis.length > 0) {
	            oldDisItemDis.remove();
	        }
	        $("#con-dpBox" + numId).before(html);
	        _dpResetInput(numId);
	    } else {
	        $("#" + itemId).html(html);
	        
	    }
	}

    function _dpRenderDialogItem(dpid, query, value, seDSeting, isSelected, callback) {
        $(".dp-loading-double-bounce").fadeIn();
        $("#dp-search-listbox-all").fadeOut();
        $("#dp-search-listbox-selected").fadeOut();
        var diaul = "#dp-search-listbox-all";
        if (isSelected && value !== "") {
            diaul = "#dp-search-listbox-selected";
            var d = _dpGetCacheDatas("dialog", value.split(_globalSettings[dpid].separator));
            if (d !== null && d !== undefined && d.IsFullCache) {
                var nd = _dpFixSelectedStatus(dpid, d.Data);
                _dpRenderDialogHtml(dpid, nd, seDSeting, diaul, true, callback);
                return;
            }
        } else {
            _dialogHasShownAllList[dpid] = true;
        }
        _dpRenderDialogDatasource(dpid, query, value, seDSeting, diaul, callback);
    }

    function _dpRenderDialogDatasource(dpid, query, value, seDSeting, diaul, callback) {
        var self = this;
        if (window._dialogItemPageIndex === undefined || window._dialogItemPageIndex === null) {
            window._dialogItemPageIndex = 1;
        }
        var itemProperties = seDSeting.properties.split(",");
        _dpAjax(_globalSettings[dpid], "search", { q: query, qp: seDSeting.search_item.queryProps, k: value, kn: seDSeting.search_item.key, s: _globalSettings[dpid].separator, p: itemProperties, ico: seDSeting.icon_setting, isd: seDSeting.isShowDefaultList, L: _globalSettings[dpid].searchDialog.pageSize, I: window._dialogItemPageIndex }, function (resp) {
            if (resp.Result === "Success") {
                var data = _dpFixSelectedStatus(dpid, resp.Data);
                _dpSetCacheDatas("dialog", data);
                var currentItemCount = window._dialogItemPageIndex * _globalSettings[dpid].searchDialog.pageSize;
                var hasShownAllData = currentItemCount >= resp.Count;
                _dpRenderDialogHtml(dpid, data, seDSeting, diaul, hasShownAllData, callback);
            } else if (resp.Result === "Empty") {
                $(diaul).html(seDSeting.emptyAlert).fadeIn();
                $(".dp-loading-double-bounce").fadeOut();
            } else {
                $(diaul).html(seDSeting.errorAlert).fadeIn();
                $(".dp-loading-double-bounce").fadeOut();
            }
        });
    }

    function _dpGetDataKeys(method, datas) {
        var keys = [];
        $.each(datas, function(i, v) {
            var key = _dpGetDataKey(method, v);
            keys.push(key);
        });
        return keys;
    }

    function _dpRenderDialogHtml(dpid, data, seDSeting, diaul, hasShownAllData, callback) {
        var val = { data: data, dpid: dpid };
        _dpRenderTemplate(seDSeting.search_item.template, val, function(html) {
            if (window._dialogItemPageIndex === 1) {
                $(diaul).html(html).fadeIn();
            } else {
                $(diaul).append(html).fadeIn();
            }
            _ListItemBindEvent();
            $(".dp-loading-double-bounce").fadeOut();
            if (callback != null && callback != undefined) {
                var d = _dpGetDataKeys("dialog", data);
                callback(hasShownAllData,d);
            }
            window._dialogItemPageIndex = window._dialogItemPageIndex + 1;
        });
    }

    function _dpDataPic(gs, settings, values) {
        if (values.length === 0 || !settings.enablePic) {
                return;
            }
            var picData = [];
            var unCache = [];
            if (_dpCache() == null) {
                unCache = values;
            } else {
                $.each(values, function (i, v) {
                    var c = _dpGetCacheDatas("pic", _dpObj2Array(v));
                    if (c.IsFullCache) {
                        var mm = { key: v, value: c.Data[0] };
                        picData.push(mm);
                    } else {
                        unCache.push(v);
                    }
                });
            }
            var data = {};
            data.K = unCache.join(",");
            data.Kn = settings.key;
            data.S = ",";
            data.Ico = settings.icon_setting;
            if (unCache.length > 0) {
                _dpAjax(gs, "pic",
                    data,
                    function (resp) {
                        if (resp.Result === "Success") {
                            _dpSetCacheDatas("pic", resp.Data);
                            $.each(resp.Data,
                                function (i, v) {
                                    var m = { key: _dpGetDataKey("pic", v), value: v };
                                    picData.push(m);
                                });
                        }
                        _dpLazyLoadPic(picData);
                    });
            } else {
                _dpLazyLoadPic(picData);
            }
    }

    function _dpLazyLoadPic(datas) {
        $.each(datas, function (i, v) {
            if (v.value.HasPhoto === "True") {
                $("img[datakey=" + v.key + "]").attr("src", v.value.StaffPhoto);
            }
        });
        
    }

    function _dpBindEvent(settings) {
        var self = this;
        $(".dpBoxWrapper").off("click").on("click", ".dpac-remove", function () {
            _dpDeleteItemFromAc(this);
        });
        $(".dpac-selected").off("click").on("click", "label", function () {
            var k = $(this).parent().find("span").attr("datakey");
            var v = $(this).text();
            if (settings.displayOnClick != null && settings.displayOnClick != undefined) {
               settings.displayOnClick(k, v, this);
            }
        });

        $(".dpBtn").off("click").on("click", function () {
            _dpBindDpBtnEvent($(this));
        });

        $(window).on("scroll resize", function () {
            _dpDelay(function () {
                $.each($(".dpBoxWrapper"), function (i, v) {
                    _dpResetInput($(v).attr("dpid"));
                });
            },500);
            
        });

        $(".dpBox").off('keyup').on('keyup', function (evt) {
            var self = this;
            var dpid = $(this).parent().parent().attr("dpid");
            var c = evt.keyCode;
            var v = $(self).val();
            if (c === 37 || c === 38 || c === 39 || c === 40 || c === 27 || c === 17) {
            } else {
                var ul = $(self).siblings();
                ul.html(_service.autocomplete_setting.loadingAlert).css("display", "block");;
                _dpDelay(function () {
                    var snum =  _dpGenerateMixed(6);
                    _autoCompleteLock[dpid] = snum;
                    _dpDataInputSearch(dpid, self, v, snum);
                }, _service.autocomplete_setting.keyUpDelayTime);
            }
        });

        $(".dpBox").off('keydown').on('keydown', function (evt) {
            var self = this;
            var v = $(this).val();
            var c = evt.keyCode;
            if (c === 37 || c === 38 || c === 39 || c === 40 || c === 27 || c === 13 || (v === "" && c === 8)) {
                if (c === 8) {
                    var el = $(self).parent().prev().find(".dpac-remove");
                    if (el.length === 1) {
                        _dpDeleteItemFromAc(el[0]);
                    }

                    return;
                }
                var sel = $(self).siblings().find(".dp-li-selected");
                if (c === 27) {
                    $(self).val("").siblings().html("");
                    return;
                } else if (c === 13) {
                    if (sel.length === 1) {
                        _dpPushAcToItem(sel);
                    }
                    return;
                }

                if (sel.length === 0) {
                    if (c === 38) {
                        var el1 = $(self).siblings().find(".dpac-li:last").eq(0);
                        _dpSetSelect(el1);
                    } else if (c === 40) {
                        var el2 = $(self).siblings().find(".dpac-li:first").eq(0);
                        _dpSetSelect(el2);
                    }
                } else {
                    if (c === 38) {
                        _dpSetSelect(sel.prev());
                    } else if (c === 40) {
                        _dpSetSelect(sel.next());
                    }
                }
            }
        });

        $(".dpComplete").off("click").on("click", "li.dpac-li", function () {
            _dpPushAcToItem(this);
        });
        $(".dpComplete").off("mouseover").on("mouseover", "li.dpac-li", function () {
            _dpSetSelect(this);
            $(this).parent().parent().find(".dpBox").focus();
        });

    }

    function _ListItemBindEvent() {
        var self = this;
        $('.list-group-item').off("click").on("click", function (event) {
            var dpid = $(this).attr("dpid");
            var currentCount = _dialogCurrentValues[dpid].length;
            if (currentCount < _globalSettings[dpid].maxCount || _globalSettings[dpid].maxCount === 0 || $(this).hasClass('selected')) {
                $(this).toggleClass('selected');
                var selectCount = 0;
                var value = $(this).find(".dpac-li-value").attr("key");
                var valueArray = [];
                valueArray.push(value.toLowerCase());
                if ($(this).hasClass('selected')) {
                    $("#dp-search-listbox-selected").append($(this).prop('outerHTML'));
                    _dialogCurrentValues[dpid] = _dialogCurrentValues[dpid].concat(valueArray).uniquelize();
                    _ListItemBindEvent();
                } else {
                    if (_dialogCurrentMode[dpid] === "selected") {
                        $("#dp-search-listbox-all").find("li[datakey='" + value + "']").toggleClass('selected');
                        $(this).remove();
                    } else {
                        var item = $("#dp-search-listbox-selected").find("li[datakey='" + value + "']");
                        item.remove();
                    }
                    _dialogCurrentValues[dpid] = _dialogCurrentValues[dpid].uniquelize().each(function (o) { return valueArray.contains(o) ? null : o });
                }
                selectCount = _dialogCurrentValues[dpid].length;
                _dpCounter(selectCount);
            }
        });
    }


    function _dpBindDpBtnEvent(el) {
        var dpid = el.attr("dpid");
        _dialogHasShownAllList[dpid] = false;
        window._dialogItemPageIndex = 1;
        var seDSeting = _service.search_dialog_setting;
        _dpRenderTemplate(seDSeting.template, { DpId: dpid, Text: _globalSettings[dpid].searchDialog.inputText }, function (html) {
            _dialog = _dpBuildDialog(dpid, seDSeting, html);
            _dialog.open();
            var btn = _dialog.getButton("dp-dialogbtn-showtype");
            if (_dialogCurrentMode[dpid] === "selected") {
                btn.html("Back to search");
            } else {
                btn.html("Selected");
            }
        });
    }

    function _dpBuildDialog(dpid, seDSeting, html) {
        var self = this;
        var value = _dpDataValue(dpid);
        var varArry = [];
        _dialogCurrentValues[dpid] = [];
        if (value !== "") {
            varArry = value.split(_globalSettings[dpid].separator);
            $.each(varArry, function(i, v) {
                _dialogCurrentValues[dpid].push(v.toLowerCase());
            });
        }
       
        var searchDialog = new BootstrapDialog({
            nl2br: false,
            message:function(dia){
                var $content = $(html);
                return $content;
            },
            closable: seDSeting.closable,
            draggable: seDSeting.draggable,
            closeByBackdrop: seDSeting.closeByBackdrop,
            closeByKeyboard: seDSeting.closeByKeyboard,
            buttons: [
                {
                    icon: '',
                    label: 'Ok',
                    cssClass: 'dpsend',
                    autospin: true,
                    action: function (dialogRef) {
                        var v = [];
                        $.each($("#dp-search-listbox-selected").find("li.selected"), function () {
                            var k = $(this).find(".dpac-li-value").attr("key");
                            if(v.indexOf(k)===-1)
                                v.push(k);
                        });
                        var newIds = v.join(_globalSettings[dpid].separator);
                        $("input[dpid=" + dpid + "]").val(newIds);
                        _dpRenderDisplayItem(dpid, newIds);
                        if (v.length >= _globalSettings[dpid].maxCount) {
                            _dpSetAcStatus(dpid, "disable");
                        }
                        dialogRef.close();
                    }
                },
                {
                    id: "dp-dialogbtn-showtype",
                    label: '',
                    action: function (dialogRef) {
                        $("#dp-search-listbox-selected").slideToggle("fast");
                        $("#dp-search-listbox-all").slideToggle("fast");
                        var btn = dialogRef.getButton("dp-dialogbtn-showtype");
                        if (_dialogCurrentMode[dpid] === "selected") {
                             btn.html("Selected");
                             _dialogCurrentMode[dpid] = "all";
                             if (_service.search_dialog_setting.isShowDefaultList && !_dialogHasShownAllList[dpid]) {
                                _dpRenderDialogItem(dpid, '', '', seDSeting, false, function (hasShownAll, datakeys) {
                                    _dialogHasShownAll[dpid] = hasShownAll;
                                    _dpDataPic(_globalSettings[dpid],seDSeting, datakeys);
                                 });
                             }
                        } else {
                            btn.html("Back to search");
                            _dialogCurrentMode[dpid] = "selected";
                        }
                    }
                },
                {
                    label: 'Close',
                    action: function (dialogRef) {
                        dialogRef.close();
                    }
                }
            ],
            onshow: function (dialogRef) {
                dialogRef.enableButtons(false);
                var btn = dialogRef.getButton("dp-dialogbtn-showtype");
                if (varArry !== undefined && varArry !== null && varArry.length > 0 && varArry[0] !== "") {
                    btn.html("Back to search");
                    _dialogCurrentMode[dpid] = "selected";
                } else {
                    btn.html("Selected");
                    _dialogCurrentMode[dpid] = "all";
                }
            },
            onshown: function (dialogRef) {
               
                var isSelected = false;
                if (varArry !== undefined && varArry !== null && varArry.length > 0 && varArry[0] !== "") {
                    _dpCounter(varArry.length);
                    isSelected = true;
                }
                $("#dp-search-go").on("click", function (e) {
                    var dpid = $(this).attr("dpid");
                    var query = $("#dp-search-inputbox").val();
                    e.preventDefault();
                    window._dialogItemPageIndex = 1;
                    _dialogHasShownAll[dpid] = false;
                    
                    var btn = _dialog.getButton("dp-dialogbtn-showtype");
                    if (_dialogCurrentMode[dpid] === "selected") {
                        btn.html("Selected");
                        _dialogCurrentMode[dpid] = "all";
                    }
                    $("#dp-search-listbox-all").scrollTop(0);
                    _dpRenderDialogItem(dpid, query, '', seDSeting, false, function(hasShownAll,datakeys) {
                        _dialogHasShownAll[dpid] = hasShownAll;
                        _dpDataPic(_globalSettings[dpid],seDSeting, datakeys);
                    });
                });
                $("#dp-search-listbox-all").scroll(function () {
                    var query = $("#dp-search-inputbox").val();
                    var $this = $(this),
                    viewH = $(this).height(),
                    contentH = $(this).get(0).scrollHeight,
                    scrollTop = $(this).scrollTop();
                    if (scrollTop / (contentH - viewH) >= 0.95 && (!_dialogLoadMore[dpid] || _dialogLoadMore[dpid] === undefined) && (!_dialogHasShownAll[dpid]|| _dialogHasShownAll[dpid]===undefined)) {
                        _dialogLoadMore[dpid] = true;
                        $(".dp-loading-double-bounce").fadeIn();
                        _dpRenderDialogDatasource(dpid, query, '', seDSeting, "#dp-search-listbox-all", function (hasShownAll, datakeys) {
                            _dialogLoadMore[dpid] = false;
                            _dialogHasShownAll[dpid] = hasShownAll;
                            $(".dp-loading-double-bounce").fadeOut();
                            _dpDataPic(_globalSettings[dpid],seDSeting, datakeys);
                        });
                    }
                });
                window._dialogItemPageIndex = 1;
                _dpRenderDialogItem(dpid, '', value, seDSeting, isSelected, function (hasShownAll, datakeys) {
                    _dialogHasShownAll[dpid] = hasShownAll;
                    _dpDataPic(_globalSettings[dpid],seDSeting, datakeys);
                });
                dialogRef.enableButtons(true);
            }
        });
        searchDialog.setTitle(seDSeting.title);
        return searchDialog;
    }

    function _dpCounter(selectCount) {
        if (selectCount > 0) {
            $('.dpsend').addClass('selected');
        }
        else {
            $('.dpsend').removeClass('selected');
        }
        $('.dpsend').attr('data-counter', selectCount);
    }

    //Auto Completed
    function _dpSetAcStatus(dpid, status) {
        if (status === "enable") {
            $("#con-dpBox" + dpid).removeClass("dp-displayNone");
        } else {
            $("#con-dpBox" + dpid).addClass("dp-displayNone");
        }
    }

    function _dpPushAcToItem(el) {
        var self = this;
        var item = $(el).find(".dpac-li-value");
        var dpid = $(el).parent().parent().parent().attr("dpid");
        var key = item.attr("key");
        if (key !== undefined) {
            var value = item.attr("value");
            var obj = {};
            obj[_service.autocomplete_setting.key] = key;
            obj[_service.autocomplete_setting.value] = value;
            if (_globalSettings[dpid].acItemOnClick != null && _globalSettings[dpid].acItemOnClick != undefined) {
                _globalSettings[dpid].acItemOnClick(obj);
            }
            _dpAddDisplayItem(obj, dpid);
            $(el).parent().parent().find(".dpBox").val("").focus();
        }
        var currentValue = _dpGetValue(dpid);
        _dpCloseAc(dpid);
        if (currentValue.length < _globalSettings[dpid].maxCount || _globalSettings[dpid].maxCount === 0) {
            _dpSetAcStatus(dpid, "enable");
        } else {
            _dpSetAcStatus(dpid, "disable");
        }
    }

    function _dpCloseAc(dpid) {
        $("#con-dpBox" + dpid).find("ul").css("display", "none");
    }

    function _dpDeleteItemFromAc(el) {
        var self = this;
        var dpid = $(el).parent().parent().attr("dpid");
        var key = $(el).attr("datakey");
        var v = $(el).parent().find("label").text();
        var isD = true;
        if (_globalSettings[dpid].displayOnDeleting != null && _globalSettings[dpid].displayOnDeleting != undefined) {
            isD = _globalSettings[dpid].displayOnDeleting(key, v, $(el).parent());
        }
        if (isD) {
            var datas = _dpDataValue(dpid).split(_globalSettings[dpid].separator);
            _dpArrayRemove(datas, key);
            _dpDataValue(dpid, datas.join(_globalSettings[dpid].separator));
            $(el).parent().fadeOut().remove();
            _dpResetInput(dpid);
            if (_globalSettings[dpid].displayOnDeleted != null && _globalSettings[dpid].displayOnDeleted != undefined) {
                _globalSettings[dpid].displayOnDeleted(key, v, $(el).parent());
            }
           
            var currentValue = _dpGetValue(dpid);
            if (currentValue.length < _globalSettings[dpid].maxCount || _globalSettings[dpid].maxCount === 0) {
                _dpSetAcStatus(dpid, "enable");
            } else {
                _dpSetAcStatus(dpid, "disable");
            }
        }
    }

    function _dpFixSelectedStatus(dpid, datas) {
        var diaItem = _service.search_dialog_setting.search_item;
        var sev = _dialogCurrentValues[dpid];
        if (sev == null || sev.length === 0) {
            sev = _dpGetValue(dpid);
        }
        var newData = [];
        var removeItem = [];
        var lowerSev = [];
        $.each(sev, function(i, s) {
            lowerSev.push(s.toLowerCase());
        });        
        $.each(lowerSev, function (is, vs) {
            $.each(datas, function (id, vd) {
                if (vd !== undefined) {
                    var vk = vd[diaItem.key].toLowerCase();
                    if (lowerSev.indexOf(vk) > -1) {
                        vd["IsSelected"] = "selected";
                    } else {
                        vd["IsSelected"] = "";
                    }
                    if (vs === vk) {
                        newData.push(vd);
                        removeItem.push(id);
                    }
                }
                
            });
        });
        $.each(removeItem, function(i, v) {
            delete datas[v];
        });
        $.each(datas, function(i, v) {
            if (v !== undefined) {
                newData.push(v);
            }
        });
        return newData;
    }

    function _dpSetSelect(el) {
        $(el).addClass("dp-li-selected").siblings().removeClass("dp-li-selected");
    }

    function _dpAddDisplayItem(data, dpid) {
        var self = this;
        var disItem = _service.display_item_setting;
        _dpSetValue(dpid, _dpObj2Array(data[_service.autocomplete_setting.key]));
        var d = { data: _dpObj2Array(data), readOnly: _globalSettings[dpid].readOnly }
        _dpRenderTemplate(disItem.template,d , function (html) {
            $("#con-dpBox" + dpid).before(html);
            _dpResetInput(dpid);
        });
    }

    function _dpEmptyValue(dpid) {
        $("input[dpid=" + dpid + "]").val("");
    }

    function _dpSetValue(dpid, value, isReplace) {
        var self = this;
        var ov = _dpGetValue(dpid);
        if (ov[0] === "" || ov[0] === null || ov[0] === undefined || isReplace) {
            var varry = value;
            varry = _dpArrayUnique(varry);
            if (varry.length >= _globalSettings[dpid].maxCount) {
                varry = varry.slice(0, _globalSettings[dpid].maxCount);
                _dpSetAcStatus(dpid, "disable");
            }
            $("input[dpid=" + dpid + "]").val(varry.join(_globalSettings[dpid].separator));
        }
        else {
            var v = ov.concat(value);
            v = _dpArrayUnique(v);
            if (v.length >= _globalSettings[dpid].maxCount) {
                v = v.slice(0, _globalSettings[dpid].maxCount);
                _dpSetAcStatus(dpid, "disable");
            }
            $("input[dpid=" + dpid + "]").val(v.join(_globalSettings[dpid].separator));
        }
        
    }

    function _dpGetValue(dpid) {
        var self = this;
        var v = $("input[dpid=" + dpid + "]").val();
        if (v === "") {
            return [];
        }
        return v.split(_globalSettings[dpid].separator);
    }

    function _dpDataInputSearch(dpid, el, keyword, snum) {
        var self = this;
        var ul = $(el).siblings();

        if (keyword === "") {
            _dpCloseAc(dpid);
            ul.html("");
            _autoCompleteLock[dpid] = undefined;
            return;
        }
        var autocomplete = _service.autocomplete_setting;
        var selected = _dpGetValue(dpid);

        _dpAjax(_globalSettings[dpid],"search", { q: keyword, p: autocomplete.properties.split(","), qp: _service.autocomplete_setting.queryProps, ig: selected, l: _globalSettings[dpid].autocomplete.limit }, function (resp) {
            if (_autoCompleteLock[dpid] === snum) {
                if (resp.Result === "Success") {
                    if (resp.Data.length === 0) {
                        ul.html(autocomplete.emptyAlert);

                    } else if (resp.Data.length === 1 && _globalSettings[dpid].autocomplete.autoFill) {
                        var obj = {};
                        obj[autocomplete.key] = resp.Data[0][autocomplete.key];
                        obj[autocomplete.value] = resp.Data[0][autocomplete.value];
                        _dpAddDisplayItem(obj, dpid);
                        $(el).val("").focus();
                        _dpCloseAc(dpid);

                    } else {
                        $.each(resp.Data, function (i, v) {
                            var text = v[autocomplete.highlight.displayPropertiy];
                            v[autocomplete.highlight.displayPropertiy + "Display"] = _dpHighlight(keyword, text);
                        });
                        _dpRenderTemplate(autocomplete.template, resp.Data, function (html) {
                            ul.html(html).css("display", "block");
                        });
                    }
                } else if (resp.Result === "Empty") {
                    ul.html(autocomplete.emptyAlert).css("display", "block");
                } else {
                    ul.html(autocomplete.errorAlert).css("display", "block");
                }
            }
            return;
        });

    }

    function _dpDataValue(dpid, value) {
        var inp = $("input[dpid='" + dpid + "']");
        if (value == undefined) {
            return inp.val();
        } else {
            return inp.val(value);
        }
    }

    function _dpResetInput(dpid) {
        var self = this;
        if (_globalSettings[dpid].autocomplete.enable) {
            var boxs = $(".dpBox");
            boxs.val("");
            $.each(boxs, function(n, b) {
                var box = $(b);
                var acul = box.parent().find("ul").css("display", "none");
                var p = box.parent().parent();
                var r = box.parent().prevAll();
                var pw = p.width();
                var itemLength = 0;
                var maxTop = 0;
                $.each(r, function(i, v) {
                    if ($(v).offset().top >= maxTop) {
                        maxTop = $(v).offset().top;
                    }
                });
                $.each(r, function(i, v) {
                    if ($(v).offset().top === maxTop) {
                        itemLength += $(v).outerWidth();
                    }
                });
                var w;
                if (pw > itemLength + 65) {
                    w = pw - itemLength - 65;
                } else {
                    w = pw - 65;
                }
                var inp = _globalSettings[dpid].autocomplete.inputbox;
                if (inp.minwidth != null && inp.minwidth > w) {
                    w = inp.minwidth;
                }
                if (inp.maxwidth != null && inp.maxwidth < w) {
                    w = inp.maxwidth;
                }
                box.width(w);
            });
        }

    }

    function _dpGenerateMixed(n) {
        var res = "";
        for (var i = 0; i < n ; i++) {
            var id = Math.ceil(Math.random() * 35);
            res += chars[id];
        }
        return res;
    }

    function _dpArrayRemove(array, item) {
        var index = array.indexOf(item);
        if (index >= 0) {
            array.splice(index, 1);
        }
    }

    function _dpObj2Array(obj) {
        var arr = [];
        arr.push(obj);
        return arr;
    }

    function _dpHighlight(keyword, wholeword) {
        return keyword === '' ? wholeword : wholeword.replace(RegExp(_dpRegExpEscape(keyword.trim()), "gi"), "<mark>$&</mark>");
    }

    Array.prototype.each = function (fn) {
        fn = fn || Function.K;
        var a = [];
        var args = Array.prototype.slice.call(arguments, 1);
        for (var i = 0; i < this.length; i++) {
            var res = fn.apply(this, [this[i], i].concat(args));
            if (res != null) a.push(res);
        }
        return a;
    };

    Array.prototype.contains = function (suArr) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == suArr) {
                return true;
            }
        }
        return false;
    }

    Array.prototype.uniquelize = function () {
        var ra = new Array();
        for (var i = 0; i < this.length; i++) {
            if (!ra.contains(this[i])) {
                ra.push(this[i]);
            }
        }
        return ra;
    };

    Array.complement = function (a, b) {
        return Array.minus(Array.union(a, b), Array.intersect(a, b));
    };
 
    Array.intersect = function (a, b) {
        return a.uniquelize().each(function (o) { return b.contains(o) ? o : null });
    };

    Array.minus = function (a, b) {
        return a.uniquelize().each(function (o) { return b.contains(o) ? null : o });
    };

    Array.union = function (a, b) {
        return a.concat(b).uniquelize();
    };

    $.fn.datapicker = function (service, options) {
        var self = this;
        var settings = $.extend({
            width: "200px",
            height: "40px",
            separator: ",",
            autocomplete: {
                enable: true,
                autoFill: true,
                limit: 10,
                inputbox: { minwidth: 200, maxwidth: 800 }
            },
            searchDialog: {
                inputText:"Input key word here",
                pageSize: 20
            },
            maxCount:10,
            readOnly: false,
            para: {},
            displayOnClick: null,
            displayOnDeleted: null,
            displayOnDeleting: null,
            acItemOnClick: null,
            acItemCompleted: null,
            dialogSave: null,
            dialogOpen: null,
            dialogClose: null
        }, options);
        _service = serviceSettings[service];
        _template = _service.template;
        _cacheEnable = _service.cache_setting.enable;
        _cacheMethod = _service.cache_setting.cacheMethod;
        var dpids = [];
        self.each(function () {
            var numId = new Date().getTime() + _dpGenerateMixed(4);
            _globalSettings[numId] = settings;
            dpids.push(numId);
            var sself = this;
            $.when(_dpBulidBaseDiv(settings,sself, numId))
                .done(function () {
                    _dpBindEvent(settings, sself);
                   
                })
            .done(function () {

            });
        });
        self.getValue = function () {
            if (dpids.length === 0) {
                var v1 = _dpGetValue(dpids[0]);
                return v1;
            }
            else {
                var v2 = [];
                $.each(dpids, function (i, item) {
                    v2.push(_dpGetValue(item));
                });
                return v2;
            }
        };

        self.setValue = function (values) {
            $.each(dpids, function (i, dpid) {
                _dpSetValue(dpid, values.split(_globalSettings[dpid].separator), false);
                var nv = _dpGetValue(dpid);
                _dpRenderDisplayItem(dpid, nv.join(_globalSettings[dpid].separator));
            });
        };
        self.changeValue = function(values) {
            $.each(dpids, function(i, dpid) {
                _dpSetValue(dpid, values.split(_globalSettings[dpid].separator), true);
                var nv = _dpGetValue(dpid);
                _dpRenderDisplayItem(dpid, nv.join(_globalSettings[dpid].separator));
            });
        };

        self.setReadOnly = function(status) {
            
            $.each(dpids,
                function (i, dpid) {
                    if (_globalSettings[dpid].readOnly === status) {
                        return;
                    }
                    _globalSettings[dpid].readOnly = status;
                    if (status) {
                        $("#di-dpBtn" + dpid).removeClass("dpBtn").addClass("dpBtnReadOnly").off("click");
                        _dpSetAcStatus(dpid, "disable");
                    } else {
                        $("#di-dpBtn" + dpid).removeClass("dpBtnReadOnly").addClass("dpBtn");
                        $("#di-dpBtn" + dpid)
                            .off("click")
                            .on("click",
                                function() {
                                    _dpBindDpBtnEvent($(this));
                                });
                        _dpSetAcStatus(dpid, "enable");
                    }
                    var values = _dpGetValue(dpid).join(_globalSettings[dpid].separator);
                    _dpRenderDisplayItem(dpid, values);
                });

        };

        return self;
    };

}(jQuery));



!function (t, e) { "use strict"; if ("undefined" != typeof module && module.exports) { var n = "undefined" != typeof process, o = n && "electron" in process.versions; o ? t.BootstrapDialog = e(t.jQuery) : module.exports = e(require("jquery"), require("bootstrap")) } else "function" == typeof define && define.amd ? define("bootstrap-dialog", ["jquery", "bootstrap"], function (t) { return e(t) }) : t.BootstrapDialog = e(t.jQuery) }(this, function (t) { "use strict"; var e = t.fn.modal.Constructor, n = function (t, n) { e.call(this, t, n) }; n.getModalVersion = function () { var e = null; return e = "undefined" == typeof t.fn.modal.Constructor.VERSION ? "v3.1" : /3\.2\.\d+/.test(t.fn.modal.Constructor.VERSION) ? "v3.2" : /3\.3\.[1,2]/.test(t.fn.modal.Constructor.VERSION) ? "v3.3" : "v3.3.4" }, n.ORIGINAL_BODY_PADDING = parseInt(t("body").css("padding-right") || 0, 10), n.METHODS_TO_OVERRIDE = {}, n.METHODS_TO_OVERRIDE["v3.1"] = {}, n.METHODS_TO_OVERRIDE["v3.2"] = { hide: function (e) { if (e && e.preventDefault(), e = t.Event("hide.bs.modal"), this.$element.trigger(e), this.isShown && !e.isDefaultPrevented()) { this.isShown = !1; var n = this.getGlobalOpenedDialogs(); 0 === n.length && this.$body.removeClass("modal-open"), this.resetScrollbar(), this.escape(), t(document).off("focusin.bs.modal"), this.$element.removeClass("in").attr("aria-hidden", !0).off("click.dismiss.bs.modal"), t.support.transition && this.$element.hasClass("fade") ? this.$element.one("bsTransitionEnd", t.proxy(this.hideModal, this)).emulateTransitionEnd(300) : this.hideModal() } } }, n.METHODS_TO_OVERRIDE["v3.3"] = { setScrollbar: function () { var t = n.ORIGINAL_BODY_PADDING; this.bodyIsOverflowing && this.$body.css("padding-right", t + this.scrollbarWidth) }, resetScrollbar: function () { var t = this.getGlobalOpenedDialogs(); 0 === t.length && this.$body.css("padding-right", n.ORIGINAL_BODY_PADDING) }, hideModal: function () { this.$element.hide(), this.backdrop(t.proxy(function () { var t = this.getGlobalOpenedDialogs(); 0 === t.length && this.$body.removeClass("modal-open"), this.resetAdjustments(), this.resetScrollbar(), this.$element.trigger("hidden.bs.modal") }, this)) } }, n.METHODS_TO_OVERRIDE["v3.3.4"] = t.extend({}, n.METHODS_TO_OVERRIDE["v3.3"]), n.prototype = { constructor: n, getGlobalOpenedDialogs: function () { var e = []; return t.each(o.dialogs, function (t, n) { n.isRealized() && n.isOpened() && e.push(n) }), e } }, n.prototype = t.extend(n.prototype, e.prototype, n.METHODS_TO_OVERRIDE[n.getModalVersion()]); var o = function (e) { this.defaultOptions = t.extend(!0, { id: o.newGuid(), buttons: [], data: {}, onshow: null, onshown: null, onhide: null, onhidden: null }, o.defaultOptions), this.indexedButtons = {}, this.registeredButtonHotkeys = {}, this.draggableData = { isMouseDown: !1, mouseOffset: {} }, this.realized = !1, this.opened = !1, this.initOptions(e), this.holdThisInstance() }; return o.BootstrapDialogModal = n, o.NAMESPACE = "bootstrap-dialog", o.TYPE_DEFAULT = "type-default", o.TYPE_INFO = "type-info", o.TYPE_PRIMARY = "type-primary", o.TYPE_SUCCESS = "type-success", o.TYPE_WARNING = "type-warning", o.TYPE_DANGER = "type-danger", o.DEFAULT_TEXTS = {}, o.DEFAULT_TEXTS[o.TYPE_DEFAULT] = "Information", o.DEFAULT_TEXTS[o.TYPE_INFO] = "Information", o.DEFAULT_TEXTS[o.TYPE_PRIMARY] = "Information", o.DEFAULT_TEXTS[o.TYPE_SUCCESS] = "Success", o.DEFAULT_TEXTS[o.TYPE_WARNING] = "Warning", o.DEFAULT_TEXTS[o.TYPE_DANGER] = "Danger", o.DEFAULT_TEXTS.OK = "OK", o.DEFAULT_TEXTS.CANCEL = "Cancel", o.DEFAULT_TEXTS.CONFIRM = "Confirmation", o.SIZE_NORMAL = "size-normal", o.SIZE_SMALL = "size-small", o.SIZE_WIDE = "size-wide", o.SIZE_LARGE = "size-large", o.BUTTON_SIZES = {}, o.BUTTON_SIZES[o.SIZE_NORMAL] = "", o.BUTTON_SIZES[o.SIZE_SMALL] = "", o.BUTTON_SIZES[o.SIZE_WIDE] = "", o.BUTTON_SIZES[o.SIZE_LARGE] = "btn-lg", o.ICON_SPINNER = "glyphicon glyphicon-asterisk", o.defaultOptions = { type: o.TYPE_PRIMARY, size: o.SIZE_NORMAL, cssClass: "", title: null, message: null, nl2br: !0, closable: !0, closeByBackdrop: !0, closeByKeyboard: !0, spinicon: o.ICON_SPINNER, autodestroy: !0, draggable: !1, animate: !0, description: "", tabindex: -1 }, o.configDefaultOptions = function (e) { o.defaultOptions = t.extend(!0, o.defaultOptions, e) }, o.dialogs = {}, o.openAll = function () { t.each(o.dialogs, function (t, e) { e.open() }) }, o.closeAll = function () { t.each(o.dialogs, function (t, e) { e.close() }) }, o.getDialog = function (t) { var e = null; return "undefined" != typeof o.dialogs[t] && (e = o.dialogs[t]), e }, o.setDialog = function (t) { return o.dialogs[t.getId()] = t, t }, o.addDialog = function (t) { return o.setDialog(t) }, o.moveFocus = function () { var e = null; t.each(o.dialogs, function (t, n) { e = n }), null !== e && e.isRealized() && e.getModal().focus() }, o.METHODS_TO_OVERRIDE = {}, o.METHODS_TO_OVERRIDE["v3.1"] = { handleModalBackdropEvent: function () { return this.getModal().on("click", { dialog: this }, function (t) { t.target === this && t.data.dialog.isClosable() && t.data.dialog.canCloseByBackdrop() && t.data.dialog.close() }), this }, updateZIndex: function () { var e = 1040, n = 1050, i = 0; t.each(o.dialogs, function (t, e) { i++ }); var s = this.getModal(), a = s.data("bs.modal").$backdrop; return s.css("z-index", n + 20 * (i - 1)), a.css("z-index", e + 20 * (i - 1)), this }, open: function () { return !this.isRealized() && this.realize(), this.getModal().modal("show"), this.updateZIndex(), this } }, o.METHODS_TO_OVERRIDE["v3.2"] = { handleModalBackdropEvent: o.METHODS_TO_OVERRIDE["v3.1"].handleModalBackdropEvent, updateZIndex: o.METHODS_TO_OVERRIDE["v3.1"].updateZIndex, open: o.METHODS_TO_OVERRIDE["v3.1"].open }, o.METHODS_TO_OVERRIDE["v3.3"] = {}, o.METHODS_TO_OVERRIDE["v3.3.4"] = t.extend({}, o.METHODS_TO_OVERRIDE["v3.1"]), o.prototype = { constructor: o, initOptions: function (e) { return this.options = t.extend(!0, this.defaultOptions, e), this }, holdThisInstance: function () { return o.addDialog(this), this }, initModalStuff: function () { return this.setModal(this.createModal()).setModalDialog(this.createModalDialog()).setModalContent(this.createModalContent()).setModalHeader(this.createModalHeader()).setModalBody(this.createModalBody()).setModalFooter(this.createModalFooter()), this.getModal().append(this.getModalDialog()), this.getModalDialog().append(this.getModalContent()), this.getModalContent().append(this.getModalHeader()).append(this.getModalBody()).append(this.getModalFooter()), this }, createModal: function () { var e = t('<div class="modal" role="dialog" aria-hidden="true"></div>'); return e.prop("id", this.getId()), e.attr("aria-labelledby", this.getId() + "_title"), e }, getModal: function () { return this.$modal }, setModal: function (t) { return this.$modal = t, this }, createModalDialog: function () { return t('<div class="modal-dialog"></div>') }, getModalDialog: function () { return this.$modalDialog }, setModalDialog: function (t) { return this.$modalDialog = t, this }, createModalContent: function () { return t('<div class="modal-content"></div>') }, getModalContent: function () { return this.$modalContent }, setModalContent: function (t) { return this.$modalContent = t, this }, createModalHeader: function () { return t('<div class="modal-header"></div>') }, getModalHeader: function () { return this.$modalHeader }, setModalHeader: function (t) { return this.$modalHeader = t, this }, createModalBody: function () { return t('<div class="modal-body"></div>') }, getModalBody: function () { return this.$modalBody }, setModalBody: function (t) { return this.$modalBody = t, this }, createModalFooter: function () { return t('<div class="modal-footer"></div>') }, getModalFooter: function () { return this.$modalFooter }, setModalFooter: function (t) { return this.$modalFooter = t, this }, createDynamicContent: function (t) { var e = null; return e = "function" == typeof t ? t.call(t, this) : t, "string" == typeof e && (e = this.formatStringContent(e)), e }, formatStringContent: function (t) { return this.options.nl2br ? t.replace(/\r\n/g, "<br />").replace(/[\r\n]/g, "<br />") : t }, setData: function (t, e) { return this.options.data[t] = e, this }, getData: function (t) { return this.options.data[t] }, setId: function (t) { return this.options.id = t, this }, getId: function () { return this.options.id }, getType: function () { return this.options.type }, setType: function (t) { return this.options.type = t, this.updateType(), this }, updateType: function () { if (this.isRealized()) { var t = [o.TYPE_DEFAULT, o.TYPE_INFO, o.TYPE_PRIMARY, o.TYPE_SUCCESS, o.TYPE_WARNING, o.TYPE_DANGER]; this.getModal().removeClass(t.join(" ")).addClass(this.getType()) } return this }, getSize: function () { return this.options.size }, setSize: function (t) { return this.options.size = t, this.updateSize(), this }, updateSize: function () { if (this.isRealized()) { var e = this; this.getModal().removeClass(o.SIZE_NORMAL).removeClass(o.SIZE_SMALL).removeClass(o.SIZE_WIDE).removeClass(o.SIZE_LARGE), this.getModal().addClass(this.getSize()), this.getModalDialog().removeClass("modal-sm"), this.getSize() === o.SIZE_SMALL && this.getModalDialog().addClass("modal-sm"), this.getModalDialog().removeClass("modal-lg"), this.getSize() === o.SIZE_WIDE && this.getModalDialog().addClass("modal-lg"), t.each(this.options.buttons, function (n, o) { var i = e.getButton(o.id), s = ["btn-lg", "btn-sm", "btn-xs"], a = !1; if ("string" == typeof o.cssClass) { var d = o.cssClass.split(" "); t.each(d, function (e, n) { -1 !== t.inArray(n, s) && (a = !0) }) } a || (i.removeClass(s.join(" ")), i.addClass(e.getButtonSize())) }) } return this }, getCssClass: function () { return this.options.cssClass }, setCssClass: function (t) { return this.options.cssClass = t, this }, getTitle: function () { return this.options.title }, setTitle: function (t) { return this.options.title = t, this.updateTitle(), this }, updateTitle: function () { if (this.isRealized()) { var t = null !== this.getTitle() ? this.createDynamicContent(this.getTitle()) : this.getDefaultText(); this.getModalHeader().find("." + this.getNamespace("title")).html("").append(t).prop("id", this.getId() + "_title") } return this }, getMessage: function () { return this.options.message }, setMessage: function (t) { return this.options.message = t, this.updateMessage(), this }, updateMessage: function () { if (this.isRealized()) { var t = this.createDynamicContent(this.getMessage()); this.getModalBody().find("." + this.getNamespace("message")).html("").append(t) } return this }, isClosable: function () { return this.options.closable }, setClosable: function (t) { return this.options.closable = t, this.updateClosable(), this }, setCloseByBackdrop: function (t) { return this.options.closeByBackdrop = t, this }, canCloseByBackdrop: function () { return this.options.closeByBackdrop }, setCloseByKeyboard: function (t) { return this.options.closeByKeyboard = t, this }, canCloseByKeyboard: function () { return this.options.closeByKeyboard }, isAnimate: function () { return this.options.animate }, setAnimate: function (t) { return this.options.animate = t, this }, updateAnimate: function () { return this.isRealized() && this.getModal().toggleClass("fade", this.isAnimate()), this }, getSpinicon: function () { return this.options.spinicon }, setSpinicon: function (t) { return this.options.spinicon = t, this }, addButton: function (t) { return this.options.buttons.push(t), this }, addButtons: function (e) { var n = this; return t.each(e, function (t, e) { n.addButton(e) }), this }, getButtons: function () { return this.options.buttons }, setButtons: function (t) { return this.options.buttons = t, this.updateButtons(), this }, getButton: function (t) { return "undefined" != typeof this.indexedButtons[t] ? this.indexedButtons[t] : null }, getButtonSize: function () { return "undefined" != typeof o.BUTTON_SIZES[this.getSize()] ? o.BUTTON_SIZES[this.getSize()] : "" }, updateButtons: function () { return this.isRealized() && (0 === this.getButtons().length ? this.getModalFooter().hide() : this.getModalFooter().show().find("." + this.getNamespace("footer")).html("").append(this.createFooterButtons())), this }, isAutodestroy: function () { return this.options.autodestroy }, setAutodestroy: function (t) { this.options.autodestroy = t }, getDescription: function () { return this.options.description }, setDescription: function (t) { return this.options.description = t, this }, setTabindex: function (t) { return this.options.tabindex = t, this }, getTabindex: function () { return this.options.tabindex }, updateTabindex: function () { return this.isRealized() && this.getModal().attr("tabindex", this.getTabindex()), this }, getDefaultText: function () { return o.DEFAULT_TEXTS[this.getType()] }, getNamespace: function (t) { return o.NAMESPACE + "-" + t }, createHeaderContent: function () { var e = t("<div></div>"); return e.addClass(this.getNamespace("header")), e.append(this.createTitleContent()), e.prepend(this.createCloseButton()), e }, createTitleContent: function () { var e = t("<div></div>"); return e.addClass(this.getNamespace("title")), e }, createCloseButton: function () { var e = t("<div></div>"); e.addClass(this.getNamespace("close-button")); var n = t('<button class="close">&times;</button>'); return e.append(n), e.on("click", { dialog: this }, function (t) { t.data.dialog.close() }), e }, createBodyContent: function () { var e = t("<div></div>"); return e.addClass(this.getNamespace("body")), e.append(this.createMessageContent()), e }, createMessageContent: function () { var e = t("<div></div>"); return e.addClass(this.getNamespace("message")), e }, createFooterContent: function () { var e = t("<div></div>"); return e.addClass(this.getNamespace("footer")), e }, createFooterButtons: function () { var e = this, n = t("<div></div>"); return n.addClass(this.getNamespace("footer-buttons")), this.indexedButtons = {}, t.each(this.options.buttons, function (t, i) { i.id || (i.id = o.newGuid()); var s = e.createButton(i); e.indexedButtons[i.id] = s, n.append(s) }), n }, createButton: function (e) { var n = t('<button class="btn"></button>'); return n.prop("id", e.id), n.data("button", e), "undefined" != typeof e.icon && "" !== t.trim(e.icon) && n.append(this.createButtonIcon(e.icon)), "undefined" != typeof e.label && n.append(e.label), n.addClass("undefined" != typeof e.cssClass && "" !== t.trim(e.cssClass) ? e.cssClass : "btn-default"), "undefined" != typeof e.hotkey && (this.registeredButtonHotkeys[e.hotkey] = n), n.on("click", { dialog: this, $button: n, button: e }, function (t) { var e = t.data.dialog, n = t.data.$button, o = n.data("button"); "function" == typeof o.action && o.action.call(n, e, t), o.autospin && n.toggleSpin(!0) }), this.enhanceButton(n), "undefined" != typeof e.enabled && n.toggleEnable(e.enabled), n }, enhanceButton: function (t) { return t.dialog = this, t.toggleEnable = function (t) { var e = this; return "undefined" != typeof t ? e.prop("disabled", !t).toggleClass("disabled", !t) : e.prop("disabled", !e.prop("disabled")), e }, t.enable = function () { var t = this; return t.toggleEnable(!0), t }, t.disable = function () { var t = this; return t.toggleEnable(!1), t }, t.toggleSpin = function (e) { var n = this, o = n.dialog, i = n.find("." + o.getNamespace("button-icon")); return "undefined" == typeof e && (e = !(t.find(".icon-spin").length > 0)), e ? (i.hide(), t.prepend(o.createButtonIcon(o.getSpinicon()).addClass("icon-spin"))) : (i.show(), t.find(".icon-spin").remove()), n }, t.spin = function () { var t = this; return t.toggleSpin(!0), t }, t.stopSpin = function () { var t = this; return t.toggleSpin(!1), t }, this }, createButtonIcon: function (e) { var n = t("<span></span>"); return n.addClass(this.getNamespace("button-icon")).addClass(e), n }, enableButtons: function (e) { return t.each(this.indexedButtons, function (t, n) { n.toggleEnable(e) }), this }, updateClosable: function () { return this.isRealized() && this.getModalHeader().find("." + this.getNamespace("close-button")).toggle(this.isClosable()), this }, onShow: function (t) { return this.options.onshow = t, this }, onShown: function (t) { return this.options.onshown = t, this }, onHide: function (t) { return this.options.onhide = t, this }, onHidden: function (t) { return this.options.onhidden = t, this }, isRealized: function () { return this.realized }, setRealized: function (t) { return this.realized = t, this }, isOpened: function () { return this.opened }, setOpened: function (t) { return this.opened = t, this }, handleModalEvents: function () { return this.getModal().on("show.bs.modal", { dialog: this }, function (t) { var e = t.data.dialog; if (e.setOpened(!0), e.isModalEvent(t) && "function" == typeof e.options.onshow) { var n = e.options.onshow(e); return n === !1 && e.setOpened(!1), n } }), this.getModal().on("shown.bs.modal", { dialog: this }, function (t) { var e = t.data.dialog; e.isModalEvent(t) && "function" == typeof e.options.onshown && e.options.onshown(e) }), this.getModal().on("hide.bs.modal", { dialog: this }, function (t) { var e = t.data.dialog; if (e.setOpened(!1), e.isModalEvent(t) && "function" == typeof e.options.onhide) { var n = e.options.onhide(e); return n === !1 && e.setOpened(!0), n } }), this.getModal().on("hidden.bs.modal", { dialog: this }, function (e) { var n = e.data.dialog; n.isModalEvent(e) && "function" == typeof n.options.onhidden && n.options.onhidden(n), n.isAutodestroy() && (delete o.dialogs[n.getId()], t(this).remove()), o.moveFocus() }), this.handleModalBackdropEvent(), this.getModal().on("keyup", { dialog: this }, function (t) { 27 === t.which && t.data.dialog.isClosable() && t.data.dialog.canCloseByKeyboard() && t.data.dialog.close() }), this.getModal().on("keyup", { dialog: this }, function (e) { var n = e.data.dialog; if ("undefined" != typeof n.registeredButtonHotkeys[e.which]) { var o = t(n.registeredButtonHotkeys[e.which]); !o.prop("disabled") && o.focus().trigger("click") } }), this }, handleModalBackdropEvent: function () { return this.getModal().on("click", { dialog: this }, function (e) { t(e.target).hasClass("modal-backdrop") && e.data.dialog.isClosable() && e.data.dialog.canCloseByBackdrop() && e.data.dialog.close() }), this }, isModalEvent: function (t) { return "undefined" != typeof t.namespace && "bs.modal" === t.namespace }, makeModalDraggable: function () { return this.options.draggable && (this.getModalHeader().addClass(this.getNamespace("draggable")).on("mousedown", { dialog: this }, function (t) { var e = t.data.dialog; e.draggableData.isMouseDown = !0; var n = e.getModalDialog().offset(); e.draggableData.mouseOffset = { top: t.clientY - n.top, left: t.clientX - n.left } }), this.getModal().on("mouseup mouseleave", { dialog: this }, function (t) { t.data.dialog.draggableData.isMouseDown = !1 }), t("body").on("mousemove", { dialog: this }, function (t) { var e = t.data.dialog; e.draggableData.isMouseDown && e.getModalDialog().offset({ top: t.clientY - e.draggableData.mouseOffset.top, left: t.clientX - e.draggableData.mouseOffset.left }) })), this }, realize: function () { return this.initModalStuff(), this.getModal().addClass(o.NAMESPACE).addClass(this.getCssClass()), this.updateSize(), this.getDescription() && this.getModal().attr("aria-describedby", this.getDescription()), this.getModalFooter().append(this.createFooterContent()), this.getModalHeader().append(this.createHeaderContent()), this.getModalBody().append(this.createBodyContent()), this.getModal().data("bs.modal", new n(this.getModal(), { backdrop: "static", keyboard: !1, show: !1 })), this.makeModalDraggable(), this.handleModalEvents(), this.setRealized(!0), this.updateButtons(), this.updateType(), this.updateTitle(), this.updateMessage(), this.updateClosable(), this.updateAnimate(), this.updateSize(), this.updateTabindex(), this }, open: function () { return !this.isRealized() && this.realize(), this.getModal().modal("show"), this }, close: function () { return !this.isRealized() && this.realize(), this.getModal().modal("hide"), this } }, o.prototype = t.extend(o.prototype, o.METHODS_TO_OVERRIDE[n.getModalVersion()]), o.newGuid = function () { return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function (t) { var e = 16 * Math.random() | 0, n = "x" === t ? e : 3 & e | 8; return n.toString(16) }) }, o.show = function (t) { return new o(t).open() }, o.alert = function () { var e = {}, n = { type: o.TYPE_PRIMARY, title: null, message: null, closable: !1, draggable: !1, buttonLabel: o.DEFAULT_TEXTS.OK, callback: null }; return e = "object" == typeof arguments[0] && arguments[0].constructor === {}.constructor ? t.extend(!0, n, arguments[0]) : t.extend(!0, n, { message: arguments[0], callback: "undefined" != typeof arguments[1] ? arguments[1] : null }), new o({ type: e.type, title: e.title, message: e.message, closable: e.closable, draggable: e.draggable, data: { callback: e.callback }, onhide: function (t) { !t.getData("btnClicked") && t.isClosable() && "function" == typeof t.getData("callback") && t.getData("callback")(!1) }, buttons: [{ label: e.buttonLabel, action: function (t) { t.setData("btnClicked", !0), "function" == typeof t.getData("callback") && t.getData("callback")(!0), t.close() } }] }).open() }, o.confirm = function () { var e = {}, n = { type: o.TYPE_PRIMARY, title: null, message: null, closable: !1, draggable: !1, btnCancelLabel: o.DEFAULT_TEXTS.CANCEL, btnOKLabel: o.DEFAULT_TEXTS.OK, btnOKClass: null, callback: null }; return e = "object" == typeof arguments[0] && arguments[0].constructor === {}.constructor ? t.extend(!0, n, arguments[0]) : t.extend(!0, n, { message: arguments[0], closable: !1, buttonLabel: o.DEFAULT_TEXTS.OK, callback: "undefined" != typeof arguments[1] ? arguments[1] : null }), null === e.btnOKClass && (e.btnOKClass = ["btn", e.type.split("-")[1]].join("-")), new o({ type: e.type, title: e.title, message: e.message, closable: e.closable, draggable: e.draggable, data: { callback: e.callback }, buttons: [{ label: e.btnCancelLabel, action: function (t) { "function" == typeof t.getData("callback") && t.getData("callback")(!1), t.close() } }, { label: e.btnOKLabel, cssClass: e.btnOKClass, action: function (t) { "function" == typeof t.getData("callback") && t.getData("callback")(!0), t.close() } }] }).open() }, o.warning = function (t, e) { return new o({ type: o.TYPE_WARNING, message: t }).open() }, o.danger = function (t, e) { return new o({ type: o.TYPE_DANGER, message: t }).open() }, o.success = function (t, e) { return new o({ type: o.TYPE_SUCCESS, message: t }).open() }, o });

/* Laura Doktorova https://github.com/olado/doT */
(function () {
    function p(b, a, d) {
        return ("string" === typeof a ? a : a.toString()).replace(b.define || h, function (a, c, e, g) { 0 === c.indexOf("def.") && (c = c.substring(4)); c in d || (":" === e ? (b.defineParams && g.replace(b.defineParams, function (a, b, l) { d[c] = { arg: b, text: l } }), c in d || (d[c] = g)) : (new Function("def", "def['" + c + "']=" + g))(d)); return "" }).replace(b.use || h, function (a, c) {
            b.useParams && (c = c.replace(b.useParams, function (a, b, c, l) {
                if (d[c] && d[c].arg && l) return a = (c + ":" + l).replace(/'|\\/g, "_"), d.__exp = d.__exp || {}, d.__exp[a] =
                d[c].text.replace(new RegExp("(^|[^\\w$])" + d[c].arg + "([^\\w$])", "g"), "$1" + l + "$2"), b + "def.__exp['" + a + "']"
            })); var e = (new Function("def", "return " + c))(d); return e ? p(b, e, d) : e
        })
    } function k(b) { return b.replace(/\\('|\\)/g, "$1").replace(/[\r\t\n]/g, " ") } var f = {
        version: "1.0.3", templateSettings: {
            evaluate: /\{\{([\s\S]+?(\}?)+)\}\}/g, interpolate: /\{\{=([\s\S]+?)\}\}/g, encode: /\{\{!([\s\S]+?)\}\}/g, use: /\{\{#([\s\S]+?)\}\}/g, useParams: /(^|[^\w$])def(?:\.|\[[\'\"])([\w$\.]+)(?:[\'\"]\])?\s*\:\s*([\w$\.]+|\"[^\"]+\"|\'[^\']+\'|\{[^\}]+\})/g,
            define: /\{\{##\s*([\w\.$]+)\s*(\:|=)([\s\S]+?)#\}\}/g, defineParams: /^\s*([\w$]+):([\s\S]+)/, conditional: /\{\{\?(\?)?\s*([\s\S]*?)\s*\}\}/g, iterate: /\{\{~\s*(?:\}\}|([\s\S]+?)\s*\:\s*([\w$]+)\s*(?:\:\s*([\w$]+))?\s*\}\})/g, varname: "it", strip: !0, append: !0, selfcontained: !1, doNotSkipEncoded: !1
        }, template: void 0, compile: void 0
    }, m; f.encodeHTMLSource = function (b) {
        var a = { "&": "&#38;", "<": "&#60;", ">": "&#62;", '"': "&#34;", "'": "&#39;", "/": "&#47;" }, d = b ? /[&<>"'\/]/g : /&(?!#?\w+;)|<|>|"|'|\//g; return function (b) {
            return b ?
            b.toString().replace(d, function (b) { return a[b] || b }) : ""
        }
    }; m = function () { return this || (0, eval)("this") }(); "undefined" !== typeof module && module.exports ? module.exports = f : "function" === typeof define && define.amd ? define(function () { return f }) : m.doT = f; var r = { start: "'+(", end: ")+'", startencode: "'+encodeHTML(" }, s = { start: "';out+=(", end: ");out+='", startencode: "';out+=encodeHTML(" }, h = /$^/; f.template = function (b, a, d) {
        a = a || f.templateSettings; var n = a.append ? r : s, c, e = 0, g; b = a.use || a.define ? p(a, b, d || {}) : b; b = ("var out='" + (a.strip ?
        b.replace(/(^|\r|\n)\t* +| +\t*(\r|\n|$)/g, " ").replace(/\r|\n|\t|\/\*[\s\S]*?\*\//g, "") : b).replace(/'|\\/g, "\\$&").replace(a.interpolate || h, function (b, a) { return n.start + k(a) + n.end }).replace(a.encode || h, function (b, a) { c = !0; return n.startencode + k(a) + n.end }).replace(a.conditional || h, function (b, a, c) { return a ? c ? "';}else if(" + k(c) + "){out+='" : "';}else{out+='" : c ? "';if(" + k(c) + "){out+='" : "';}out+='" }).replace(a.iterate || h, function (b, a, c, d) {
            if (!a) return "';} } out+='"; e += 1; g = d || "i" + e; a = k(a); return "';var arr" +
            e + "=" + a + ";if(arr" + e + "){var " + c + "," + g + "=-1,l" + e + "=arr" + e + ".length-1;while(" + g + "<l" + e + "){" + c + "=arr" + e + "[" + g + "+=1];out+='"
        }).replace(a.evaluate || h, function (a, b) { return "';" + k(b) + "out+='" }) + "';return out;").replace(/\n/g, "\\n").replace(/\t/g, "\\t").replace(/\r/g, "\\r").replace(/(\s|;|\}|^|\{)out\+='';/g, "$1").replace(/\+''/g, ""); c && (a.selfcontained || !m || m._encodeHTML || (m._encodeHTML = f.encodeHTMLSource(a.doNotSkipEncoded)), b = "var encodeHTML = typeof _encodeHTML !== 'undefined' ? _encodeHTML : (" + f.encodeHTMLSource.toString() +
        "(" + (a.doNotSkipEncoded || "") + "));" + b); try { return new Function(a.varname, b) } catch (q) { throw "undefined" !== typeof console && console.log("Could not create a template function: " + b), q; }
    }; f.compile = function (b, a) { return f.template(b, null, a) }
})();