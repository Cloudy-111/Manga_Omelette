import { innitializeSelect2, innitializeSelectize, handleSelectionChange, handleItemRemoval } from '../scriptSelectDropdown.js'
import { previewUpload } from '../scriptPreviewUpload.js'
$(document).ready(function () {
    previewUpload();

    innitializeSelect2('#select_list', 'Select Genres');
    innitializeSelectize('#select_author', 'Select Authors');
    innitializeSelectize('#select_artist', 'Select Artist');

    handleSelectionChange('.list_tag_genres', '#select_list', 'genre');
    handleSelectionChange('.list_author', '#select_author', 'author');
    handleSelectionChange('.list_artist', '#select_artist', 'artist');

    handleItemRemoval('.list_tag_genres', 'genre');
    handleItemRemoval('.list_author', 'author');
    handleItemRemoval('.list_artist', 'artist');
})