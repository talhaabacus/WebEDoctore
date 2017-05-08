/* Tree View Script*/

$(document).ready(function () {

    $(".treeview li > ul").css('display', 'none');
    $(".collapsible").click(function (e) {

        e.preventDefault();
        $(this).toggleClass("collapse expand");
        $(this).closest("li").children("ul").slideToggle();
    });




});



$(function () {
    $('.selector').contextPopup({

        title: 'Document Category',
        items: [
          {
              label: 'Create Sub-Category', icon: '../../../../Images/Category.png', action: function ($trigger, e) {
                  var triggerElement = $trigger.currentTarget;
                  getSelectedNodeValue(triggerElement, "SubCategory");
              }
          },
          {
              label: 'Create Sibling', icon: '../../../../Images/Category.png', action: function ($trigger, e) {

                  var triggerElement = $trigger.currentTarget;
                  getSelectedNodeValue(triggerElement, "Sibling");
              }
          },
          {
              label: 'Edit', icon: '../../../../Images/edit.png', action: function ($trigger, e) {
                  var triggerElement = $trigger.currentTarget;
                  getSelectedNodeValue(triggerElement, "Edit");
                  //var Self = $trigger.currentTarget;
                  //var Parent = $trigger.currentTarget;

                  //var MenuID = $("#" + Self.id).find("input:hidden[name=MenuID]").val();
                  //var ParentID = $("#" + Parent.id).find("input:hidden[name=ParentID]").val();

                  //$("#txtMenuID").val(MenuID);
                  //  $("#txtParentID").val(ParentID);
              }
          },

          {
              label: 'Delete', icon: '../../../../Images/delete.png', action: function ($trigger, e) {

                  var triggerElement = $trigger.currentTarget;
                  getSelectedNodeValue(triggerElement, "Delete");
              }
          },
          //{ label: 'Cheese', icon: '../../contextmenumaster/icons/bin-metal.png', action: function () { alert('clicked 5') } },
          //{ label: 'Bacon', icon: '../../Images/bullet.png', action: function () { alert('clicked 6') } },
        //  null, // divider
          //{ label: 'Onwards', icon: '../../Images/bullet.png', action: function () { alert('clicked 7') } },
          //{ label: 'Flutters', icon: '../../contextmenumaster/icons/cassette.png', action: function () { alert('clicked 8') } }
        ]
    });


});







function getSelectedNodeValue(CurrentID, type) {
    $("#txtCategoryName").val("");
    $("#txtCategoryDesc").val("");

    if (type == "Delete") {
        $('#DeleteCategoryConfirmation').dialog('open');
    }
    else {
        if (type == "Edit") {
            $('#AddEditCategory').dialog('option', 'title', 'Edit Record');
            var Name = $("#" + CurrentID.id).find("input:hidden[name=Name]").val();
            var Description = $("#" + CurrentID.id).find("input:hidden[name=Description]").val();
            $("#txtCategoryName").val(Name);
            $("#txtCategoryDesc").val(Description);
        }

        else
            $('#AddEditCategory').dialog('option', 'title', 'Add Sibling/Sub-Category');




        $('#AddEditCategory').dialog('open');

    }


    var MenuID = $("#" + CurrentID.id).find("input:hidden[name=MenuID]").val();
    var ParentID = $("#" + CurrentID.id).find("input:hidden[name=ParentID]").val();
   

    $("#_MenuID").val(MenuID);
    $("#_ParentID").val(ParentID);
   
    $("#actionType").val(type);

}



function ErrorDialoge(Error)
{
    $("#Error").html("");
    $("#Error").html(Error);
    $('#ErrorModelPop').dialog('open');
}

$(function () {
    $('#detailID')
      .dialog({
          autoOpen: true,
          width: 600,
          height: 400,
          position: 'center',
          resizable: true,
          draggable: true
      });
});

$(function () {
    $('#ErrorModelPop').dialog({
        title: "Error",
        autoOpen: false,
        width: 400,
        modal: true,
        height: 150,
        buttons: {
            
            "Close": function () { $(this).dialog("close"); }
        }

    });
});


$(function () {
    $('#DeleteCategoryConfirmation').dialog({
        title: "Confirmation",
        autoOpen: false,
        width: 400,
        modal: true,
        height: 150,
        buttons: {
            "Delete": function () {
                DeleteCategory();
            },
            "Close": function () { $(this).dialog("close"); }
        }

    });
});

$(function () {
    $('#AddEditDocument').dialog({
        title: "Add/Edit Document",
        autoOpen: false,
        width: 700,
        modal: true,
        height: 300,
        buttons: {
            "SAVE": function () {
                AddPatientDocument();
            },
            "Close": function () { $(this).dialog("close"); }
        }

    });
});
$(function () {
    $('#DeleteDocumentConfirmation').dialog({
        title: "Delete Document Confirmation",
        autoOpen: false,
        width: 400,
        modal: true,
        height: 150,
        buttons: {
            "Delete": function () {
                DeleteDocument();

            },
            "Close": function () { $(this).dialog("close"); }
        }

    });
});




$(function () {
    $('#AddEditCategory').dialog({
        title: "Add/Edit",
        autoOpen: false,
        width: 400,
        modal: true,
        height: 300,
        buttons: {
            "SAVE": function () {
                SaveCategory();
            },
            "Close": function () { $(this).dialog("close"); }
        }

    });
});






function AddDocument()
{
    ClearAll();
    $('#AddEditDocument').dialog('option', 'title', 'Add Document');
    $('#AddEditDocument').dialog('open');
}







function setDateToPatientDocument(id)
{
    ClearAll();
    var Json = jQuery.parseJSON($("#hdPatientDocument-" + id).val());
   
    if (Json.ELECTRONIC_LINK != null && Json.ELECTRONIC_LINK != "") {
        $("#ddlAttachementType").val(1);
        $("#txtDocumentElectronicLink").val(Json.ELECTRONIC_LINK);
        $("#txtDocumentElectronicLink").removeAttr("disabled");
    }
    else {
        $("#ddlAttachementType").val(2);
        $("#DocumentFile").removeAttr("disabled");
    }
    // alert(Json.IBCCode);
    $("#txtTitle").val(Json.TITLE);
    $("#hdPatientDocumentID").val(Json.PATIENT_DOCUMENT_ID);
    $("#txtDocumentName").val(Json.NAME);
    $("#txtDocumentDesc").val(Json.DESCRIPTION);
    $("#txtDocumentCategory").val(Json.DOC_CATEGORY_ID);
   
    $('#AddEditDocument').dialog('option', 'title', 'Edit Document');
    $('#AddEditDocument').dialog('open');
    
}


function DeletePatientDocument(id)
{
    $("#hdPatientDocumentID").val(id);
    $("#DeleteDocumentConfirmation").dialog('open');
}

function ClearAll()
{
    $("#ddlAttachementType").val(0);
    $("#DocumentFile").val("");
    $("#txtTitle").val("");
    $("#txtDocumentName").val("");
    $("#txtDocumentDesc").val("");
    $("#txtDocumentCategory").val("");
    $("#txtDocumentElectronicLink").val("");
    $("#hdPatientDocumentID").val("0");
   

}


function TypeOfDocument()
{

    var ddlType = $("#ddlAttachementType").val();
    if(ddlType==0)
    {
        $("#txtDocumentElectronicLink").prop('disabled', true);

        $("#DocumentFile").prop('disabled', true);
        $("#DocumentFile").val("");
        $("#txtDocumentElectronicLink").val("");
    }
    else  if(ddlType==1)
    {
        $("#txtDocumentElectronicLink").removeAttr('disabled');
        $("#DocumentFile").prop('disabled', true);
        $("#DocumentFile").val("");
        $("#txtDocumentElectronicLink").val("");
        
    }
    else
    {
        $("#txtDocumentElectronicLink").prop('disabled', true);
      
        $("#DocumentFile").removeAttr('disabled');
        $("#DocumentFile").val("");
        $("#txtDocumentElectronicLink").val("");
    }
}

function ViewElectronicFileLink(path) {

    debugger;
    // window.location = path;
    window.open(path);

}
jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();