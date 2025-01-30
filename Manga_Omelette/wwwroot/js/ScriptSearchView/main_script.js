import { FilterSearch } from './script_filter.js'
import { Pagination } from './script_paginationSearchView.js'
import { ClearButton } from './script_clear_button.js'
import { Popstate } from './script_popstate.js'
import { LiveSearchAuthor } from './script_live_search.js'
import { CheckAndUncheck } from './script_check_uncheck.js'

$(document).ready(function () {
    FilterSearch();
    Pagination();
    ClearButton();
    Popstate();
    LiveSearchAuthor();
    CheckAndUncheck();
})