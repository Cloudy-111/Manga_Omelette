import { FilterSearch } from './script_filter.js'
import { Pagination } from './script_paginationSearchView.js'
import { ClearButton } from './script_clear_button.js'

$(document).ready(function () {
    FilterSearch();
    Pagination();
    ClearButton();
})