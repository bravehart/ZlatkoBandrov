
(function ($) {

    var searchResulsPanelSelector = "div#search-results-panel:first";
    var ajaxFormSelector = "form#search-form:first";

    $(document).ready(function () {

        $(ajaxFormSelector).submit(function (e) {
            var postData = $(this).serializeArray();
            var formURL = $(this).attr("action");
            $.ajax(
                {
                    url: formURL,
                    type: "POST",
                    data: postData,
                    success: function (data, textStatus, jqXHR) {
                        var resultsElement = $(searchResulsPanelSelector);
                        resultsElement.html(data);
                        bindHeaderClick();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(jqXHR);
                    }
                }
            );
            e.preventDefault();
            e.stopPropagation();
        });

        bindHeaderClick();
    });

    function bindHeaderClick() {
        $("th.results-table-header").click(function () {

            // Set the order by column
            var orderByElement = $("form#search-form:first input[name=OrderBy]");
            var oldOrderByValue = orderByElement.val();

            var newOrderByValue = $(this).find("input[type=hidden]:first").val();
            orderByElement.val(newOrderByValue);

            // Set the sort direction
            var sortDirectionElement = $("form#search-form:first input[name=SortDirection]");
            var sortDirectionValue = sortDirectionElement.val();

            if (oldOrderByValue == newOrderByValue) {
                if (sortDirectionValue == "ASC") {
                    sortDirectionElement.val("DESC");
                }
                else {
                    sortDirectionElement.val("ASC");
                }
            }
            else {
                sortDirectionElement.val("ASC");
            }

            // Submit the form
            $(ajaxFormSelector).submit();
        });
    }

})(jQuery);