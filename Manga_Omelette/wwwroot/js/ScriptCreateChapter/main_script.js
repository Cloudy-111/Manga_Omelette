import { listImageUpload } from '../scriptListImageUpload.js'
import { postCreateChapter } from './script_post_createChapter.js'
import { startConnection } from '../ScriptChapterIndex/Comment_SignalR/hubConnection.js'

$(document).ready(function () {
    listImageUpload();
    postCreateChapter();
    startConnection();
})