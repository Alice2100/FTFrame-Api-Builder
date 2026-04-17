// Global Configuration
var apiBasePath = 'http://localhost:81'
var fileBasePath = apiBasePath
var imgBasePath = apiBasePath
var ftdpConfig = {
    apiBase: apiBasePath,
    fileBase: fileBasePath,
    uploadUrl: (para) => { return fileBasePath + '/api/Base/Upload' },
    uploadFileUrl: (para) => { return fileBasePath + '/api/Base/UploadFile' },
    downloadFileUrl: (para) => { return fileBasePath + '/api/Base/DownloadFile' },
    getImageUrl: (path) => { return imgBasePath + path },
    headerObjUpload: { Authorization: '' + getTokenLocal() },
    apiBase2: '',
    apiBase3: '',
    apiBase4: '',
    apiBase5: '',
    apiBase6: '',
    apiBase7: '',
    apiBase8: '',
    tokenKey: 'token',
    getTokenLocal: function (para) {
        return getTokenLocal()
    }
}
// Retrieve saved tokens on the client side
function getTokenLocal() {
    return 'ftdp'
}
/*
export {
  ftdpConfig
}*/