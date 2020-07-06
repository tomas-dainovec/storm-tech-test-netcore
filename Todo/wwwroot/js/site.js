// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    var $todoList = $('#todoList').sortable({
        items: 'li:not(li:first)',
        update: function (_, ui) {
            var newIndex = ui.item.index();

            $.post('/TodoItem/UpdateRank', {
                todoListId: $todoList.data('todo-list-id'),
                todoItemId: ui.item.data('todo-item-id'),
                newRank: newIndex,
            });
        }
    }); 

    var $newItemModal = $('#newItemModal')
        .on('shown.bs.modal', function (e) {
            var todoListId = $todoList.data('todo-list-id');
            $('.modal-body form', this).load('/TodoItem/RenderCreateFieldsPartial?todoListId=' + todoListId);
        })
        .on('hide.bs.modal', function () {
            $('.modal-body form', this).empty();
        });    

    $('#newItemForm').submit(function () {
        var $this = $(this);
        var formData = $this.serialize();

        $.post('/TodoItem/HandleCreateForm', formData, function (data, _, jqXHR) {
            if (jqXHR.status == 201) {
                $newItemModal.modal('hide');

                $.get('/TodoList/RenderListItems' + window.location.search, function (listData) {
                    $('li:first', $todoList)
                        .siblings()
                        .remove();

                    $todoList.append(listData).sortable('refresh');
                });
            } else {
                $this.html(data);
            }
        });
        
        return false;
    });
});
