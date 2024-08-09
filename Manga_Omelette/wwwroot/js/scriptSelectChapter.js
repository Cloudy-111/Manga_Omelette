export function selectChapter() {
	//Select  Chapter
	$('#selectChapter').on('change', function () {
		var selectValue = $(this).val();
		if (selectValue) window.location.href = selectValue;
	})

	var listChapter = [];
	$('#selectChapter option').each(function () {
		listChapter.push($(this).val().slice(9));
	})

	var currentIndex = listChapter.findIndex(item => item === "");
	var nextIndex = currentIndex < listChapter.length - 1 ? currentIndex + 1 : currentIndex;
	var prevIndex = currentIndex > 0 ? currentIndex - 1 : currentIndex;

	if (currentIndex === 0) {
		$('#prevChapter').prop('disabled', true);
	} else {
		$('#prevChapter').prop('disabled', false);
		$('#prevChapter').on('click', function () {
			window.location.href = `/chapter/${listChapter[prevIndex]}`;
		})
	};

	if (currentIndex === listChapter.length - 1) {
		$('#nextChapter').prop('disabled', true);
	} else {
		$('#nextChapter').prop('disabled', false);
		$('#nextChapter').on('click', function () {
			window.location.href = `/chapter/${listChapter[nextIndex]}`;
		})
	};
}