$(document).ready(function() {
	$('.displaycategories a').click(function() { // open
		var categoryContainer = $(this).parents('.categorycontainer');
		$(this).parent().hide();
		var categoryPicker = categoryContainer.children('.categorypicker');
		var editButtonsSpan = categoryContainer.children('.categoryeditbuttons');
		editButtonsSpan.show();

		var selectedCategoryIds = categoryContainer.children('#categories').val().split(",");
		if (categoryPicker.children().length == 0) {
			loadCategories(categoryPicker, selectedCategoryIds);
		}
		categoryPicker.show();
		return false;
	});
	$('.categoryeditbuttons input').click(function() { // Ok
		var categoryContainer = $(this).parents('.categorycontainer');
		var selectedCategoryIds = [];
		var selectedCategoryNames = [];
		categoryContainer.children('.categorypicker').find('input:checkbox').each(function(i, el) {
			if ($(el).attr('checked')) {
				selectedCategoryIds.push($(el).val());
				selectedCategoryNames.push($(el).next('label').text());
			}
		})
		categoryContainer.children('#categories').val(selectedCategoryIds.join(','));
		categoryContainer.find('.categorynames').text(selectedCategoryNames.join(','));
		
		$(this).parent().hide();
		categoryContainer.children('.categorypicker').hide();
		categoryContainer.children('.displaycategories').show();
		return false;
	});
	$('.categoryeditbuttons a').click(function() { // Cancel
		var categoryContainer = $(this).parents('.categorycontainer');
		$(this).parent().hide();
		categoryContainer.children('.categorypicker').hide();
		categoryContainer.children('.displaycategories').show();
		return false;
	});
});

function loadCategories(container, selectedCategoryIds) {
	$.getJSON(CuyahogaConfig.SharedControllersDir + 'Categories/GetAll/', null, function(data) {
		var html = '<ul>';
		jQuery.each(data, function(i, category) {
			html += '<li style="padding-left:' + category.Level * 18 + 'px">';
			html += '<input type="checkbox" id="cat_' + category.Id + '" name="selectedCategories" value="' + category.Id + '"';
			if (selectedCategoryIds.indexOf(category.Id.toString()) > -1) {
				html += ' checked="checked"';
			}
			html += ' />';
			html += '<label for="cat_' + category.Id + '">' + category.Name + '</label>';
			html += '</li>'
		})
		html += '</ul>';
		container.append(html);
	});
}