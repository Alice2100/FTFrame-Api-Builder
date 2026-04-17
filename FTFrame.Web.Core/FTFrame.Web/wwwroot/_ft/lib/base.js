/* eslint-disable no-mixed-spaces-and-tabs */
/* eslint-disable eqeqeq */

var ftdpBase = {
    list_json: function (config, isExport, exportMax, exportName) {
        return list_json(config, isExport, exportMax, exportName)
    },
    list_data: function (
        config,
        vm,
        func_beforeload,
        func_beforeset,
        func_afterset
    ) {
        return list_data(
            config,
            vm,
            func_beforeload,
            func_beforeset,
            func_afterset
        )
    },
    list_reset: function (sdata, orisdata) {
        return list_reset(sdata, orisdata)
    },
    list_excel: function (config, vm, maxRow, exportName) {
        return list_excel(config, vm, maxRow, exportName)
    },
    optionsJson: function (config, vm, apiUrl, callBack) {
        return optionsJson(config, vm, apiUrl, callBack)
    },
    form_fill: function (
        config,
        vm,
        form_paras,
        func,
        func_beforeget,
        func_beforeset,
        func_afterset
    ) {
        return form_fill(
            config,
            vm,
            form_paras,
            func,
            func_beforeget,
            func_beforeset,
            func_afterset
        )
    },
    form_reset: function (form, oriform, allEmpty) {
        return form_reset(form, oriform, allEmpty)
    },
    form_submit: function (config, vm, form_paras, json, func) {
        return form_submit(config, vm, form_paras, json, func)
    },
    normal_get: function (vm, ftdpConfig, apiPath, getParas, func) {
        return normal_get(vm, ftdpConfig, apiPath, getParas, func)
    },
    normal_post: function (vm, ftdpConfig, apiPath, postParas, json, func) {
        return normal_post(vm, ftdpConfig, apiPath, postParas, json, func)
    },
    normal_validate: function (vm, str, pattern, message, callback) {
        return normal_validate(vm, str, pattern, message, callback)
    },
    file_download: function (vm, ftdpConfig, filepath) {
        file_download(vm, ftdpConfig, filepath)
    },
    toValueArray: function (arr) {
        return toValueArray(arr)
    },
    toObjectArray: function (arr, key) {
        return toObjectArray(arr, key)
    }
}
// 
function list_json(config, isExport, exportMax, exportName) {
    var jsons = {
        orderBy: config.orderBy,
        orderType: config.orderType,
        schText: config.schText,
        schStrict: config.schStrict,
        pageSize: config.pageSize,
        numType: '',
        pageNum: config.pageNum
    }
    if (isExport) {
        jsons.isExport = '1'
        if (exportMax != null) jsons.exportMax = exportMax
        if (exportName != null) jsons.exportName = exportName
    }
    return jsons
}
// 
function list_data(config, vm, func_beforeload, func_beforeset, func_afterset) {
    if (!func_beforeload()) return
    vm.$set(vm.page, 'loading', true)
    var header = { 'Content-Type': 'application/json; charset=UTF-8' }
    header[config.tokenKey] = config.tokenVal
    var paras = config.apiPara
    if (paras != '' && paras[0] != '/') paras = '/' + paras
    axios({
        url: config.apiUrl + paras,
        method: 'post',
        headers: header,
        data: JSON.stringify(list_json(config))
    })
        .then(res => {
            console.log(res.data)
            if (res.data.code == 200) {
                if (func_beforeset(res.data)) {
                    vm.$set(vm.gridData, 'list', res.data.data.list)
                    vm.$set(vm.page, 'count', res.data.data.page.pageCount)
                    vm.$set(vm.page, 'total', res.data.data.page.count)
                    func_afterset(res.data)
                }
            } else {
                vm.$message.error(res.data.msg)
            }
            vm.$set(vm.page, 'loading', false)
        })
        .catch(Error => {
            console.log(Error)
        })
}
// 
function list_reset(sdata, orisdata) {
    var keys = Object.keys(sdata)
    var osdata = JSON.parse(orisdata)
    for (var i = 0; i < keys.length; i++) {
        if (osdata[keys[i]] != null) {
            if (sdata[keys[i]]['selValue'] != null) { sdata[keys[i]]['selValue'] = osdata[keys[i]]['selValue'] } else sdata[keys[i]] = osdata[keys[i]]
        }
    }
}
// 
function list_excel(config, vm, maxRow, exportName) {
    var header = {
        'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
    }
    header[config.tokenKey] = config.tokenVal
    var paras = config.apiPara
    if (paras != '' && paras[0] != '/') paras = '/' + paras
    axios({
        url: config.apiUrl + paras,
        method: 'post',
        responseType: 'arraybuffer',
        headers: header,
        data: JSON.stringify(list_json(config, 1, maxRow, exportName))
    })
        .then(res => {
            const filename = res.headers['content-disposition'].match(
                /filename=(.*)/
            )[1]
            const blob = new Blob([res.data], { type: res.headers['content-type'] })
            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                // 
                window.navigator.msSaveBlob(blob, decodeURI(filename))
            } else {
                // 
                const blobURL = window.URL.createObjectURL(blob)
                // 
                const tempLink = document.createElement('a')
                tempLink.style.display = 'none'
                tempLink.href = blobURL
                tempLink.setAttribute('download', decodeURI(filename))
                // 
                if (typeof tempLink.download === 'undefined') {
                    tempLink.setAttribute('target', '_blank')
                }
                // 
                document.body.appendChild(tempLink)
                tempLink.click()
                document.body.removeChild(tempLink)
                // 
                window.URL.revokeObjectURL(blobURL)
            }
        })
        .catch(Error => {
            console.log(Error)
        })
}
// 
function optionsJson(config, vm, apiUrl, callBack) {
    var header = {
        'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
    }
    header[config.tokenKey] = config.tokenVal
    axios({
        url: apiUrl,
        method: 'post',
        headers: header,
        data: '{}'
    })
        .then(res => {
            if (res.data.code == 200) {
                var json = []
                for (var i = 0; i < res.data.data.list.length; i++) {
                    var obj = res.data.data.list[i]
                    var keys = Object.keys(obj)
                    var _value = null
                    var _label = null
                    if (keys.length > 2) {
                        _value = obj[keys[1]]
                        _label = obj[keys[2]]
                    } else {
                        _value = obj[keys[0]]
                        _label = obj[keys[1]]
                    }
                    json[json.length] = { value: _value, label: _label }
                }
                if (callBack != null) callBack(json)
            } else {
                vm.$message.error(res.data.msg)
            }
        })
        .catch(Error => {
            console.log(Error)
        })
}
// 
function form_fill(
    config,
    vm,
    form_paras,
    func,
    func_beforeget,
    func_beforeset,
    func_afterset
) {
    if (config.apiGet == null) return
    if (!func_beforeget()) return
    vm.$set(vm.page, 'loading', true)
    var paras = form_paras
    if (paras != '' && paras[0] != '/') paras = '/' + paras
    var header = {
        'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
    }
    header[config.tokenKey] = config.tokenVal
    axios({
        url: config.apiGet + paras,
        method: 'get',
        headers: header
    })
        .then(res => {
            console.log(res.data)
            if (res.data.code == 200) {
                if (func_beforeset(res.data)) {
                    var detail = res.data.data.detail
                    func(detail)
                    func_afterset(res.data)
                }
            } else {
                vm.$message.error(res.data.msg)
            }
            vm.$set(vm.page, 'loading', false)
        })
        .catch(Error => {
            console.log(Error)
        })
}
// 
function form_reset(form, oriform, allEmpty) {
    var keys = Object.keys(form)
    if (allEmpty) {
        for (var i = 0; i < keys.length; i++) {
            if (form[keys[i]]['selValue'] != null) form[keys[i]]['selValue'] = ''
            else form[keys[i]] = ''
        }
        return
    }
    var oform = JSON.parse(oriform)
    for (i = 0; i < keys.length; i++) { if (oform[keys[i]] != null) form[keys[i]] = oform[keys[i]] }
}
// 
function form_submit(config, vm, form_paras, json, func) {
    if (config.apiSet == null) return
    vm.$set(vm.page, 'loading', true)
    var paras = form_paras
    if (paras != '' && paras[0] != '/') paras = '/' + paras
    var header = { 'Content-Type': 'application/json; charset=UTF-8' }
    header[config.tokenKey] = config.tokenVal
    axios({
        url: config.apiSet + paras,
        method: 'post',
        headers: header,
        data: JSON.stringify(json)
    })
        .then(res => {
            console.log(res.data)
            if (res.data.code == 200) {
                vm.$message({ message: 'Operation successfully', type: 'success' })
                if (func != null) func()
            } else {
                vm.$message.error(res.data.msg)
            }
            vm.$set(vm.page, 'loading', false)
        })
        .catch(Error => {
            console.log(Error)
        })
}
// 
function normal_get(vm, ftdpConfig, apiPath, getParas, func) {
    vm.$set(vm.page, 'loading', true)
    var paras = getParas
    if (paras != '' && paras[0] != '/') paras = '/' + paras
    var header = {
        'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
    }
    header[ftdpConfig.tokenKey] = ftdpConfig.getTokenLocal()
    axios({
        url: ftdpConfig.apiBase + apiPath + paras,
        method: 'get',
        headers: header
    })
        .then(res => {
            console.log(res.data)
            if (res.data.code == 200) {
                if (func != null) func(res.data.data.detail)
            } else {
                vm.$message.error(res.data.msg)
            }
            vm.$set(vm.page, 'loading', false)
        })
        .catch(Error => {
            console.log(Error)
        })
}
// 
function normal_post(vm, ftdpConfig, apiPath, postParas, json, func) {
    vm.$set(vm.page, 'loading', true)
    var paras = postParas
    if (paras != '' && paras[0] != '/') paras = '/' + paras
    var header = { 'Content-Type': 'application/json; charset=UTF-8' }
    header[ftdpConfig.tokenKey] = ftdpConfig.getTokenLocal()
    axios({
        url: ftdpConfig.apiBase + apiPath + paras,
        method: 'post',
        headers: header,
        data: json == null ? '' : JSON.stringify(json)
    })
        .then(res => {
            console.log(res.data)
            if (res.data.code == 200) {
                vm.$message({ message: 'Operation successfully', type: 'success' })
                if (func != null) func(res.data.data)
            } else {
                vm.$message.error(res.data.msg)
            }
            vm.$set(vm.page, 'loading', false)
        })
        .catch(Error => {
            console.log(Error)
        })
}
// 
function normal_validate(vm, str, pattern, message, callback) {
    str = (str == null) ? '' : str.trim()
    if (pattern.test(str)) {
        return true
    } else {
        vm.$message.warning(message)
        if (callback != null) callback(vm)
        return false
    }
}
// Object array to value array
function toValueArray(arr) {
    var valueArr = []
    for (var i = 0; i < arr.length; i++)valueArr[i] = arr[i][Object.keys(arr[i])[0]] + ''
    return valueArr
}
// Value array to object array
function toObjectArray(arr, key) {
    var objectArr = []
    for (var i = 0; i < arr.length; i++) { objectArr[i] = {}; objectArr[i][key] = arr[i] }
    return objectArr
}
// 
function file_download(vm, ftdpConfig, filepath) {
    var header = {
        'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
    }
    header[ftdpConfig.tokenKey] = ftdpConfig.getTokenLocal()
    axios({
        url: ftdpConfig.downloadFileUrl(),
        method: 'post',
        responseType: 'arraybuffer',
        headers: header,
        data: '{"file":"' + filepath + '"}'
    })
        .then(res => {
            const filename = res.headers['content-disposition'].match(
                /filename=(.*)/
            )[1]
            const blob = new Blob([res.data], { type: res.headers['content-type'] })
            if (typeof window.navigator.msSaveBlob !== 'undefined') {
                // 
                window.navigator.msSaveBlob(blob, decodeURI(filename))
            } else {
                // 
                const blobURL = window.URL.createObjectURL(blob)
                // 
                const tempLink = document.createElement('a')
                tempLink.style.display = 'none'
                tempLink.href = blobURL
                tempLink.setAttribute('download', decodeURI(filename))
                // 
                if (typeof tempLink.download === 'undefined') {
                    tempLink.setAttribute('target', '_blank')
                }
                // 
                document.body.appendChild(tempLink)
                tempLink.click()
                document.body.removeChild(tempLink)
                // 
                window.URL.revokeObjectURL(blobURL)
            }
        })
        .catch(Error => {
            console.log(Error)
        })
}