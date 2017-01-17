jQuery(function($) {
    "use strict";

    /*=======================================
	   General
	=========================================*/
    
    
    
    /*=================================
    Image upload
    ==================================*/
    function readURL1(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#build-thumb1').attr('style', "background-image:url("+e.target.result+")" );
                $('#build-thumb0').attr('style', "background-image:url("+e.target.result+")" );
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    $("body").on("change",".build-file1",function(e){
        var $this = $(this);
        readURL1(this);
    });
    function readURL2(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#build-thumb2').attr('style', "background-image:url("+e.target.result+")" );
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    
    $("body").on("change",".build-file2",function(e){
        var $this = $(this);
        readURL2(this);
    });
    
    function readURL3(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#build-thumb3').attr('style', "background-image:url("+e.target.result+")" );
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    $("body").on("change",".build-file3",function(e){
        var $this = $(this);
        readURL3(this);
        
    });
    
    function readURL4(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#build-thumb4').attr('style', "background-image:url("+e.target.result+")" );
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    $("body").on("change",".build-file4",function(e){
        var $this = $(this);
        var fileName = $(this).val();
        var fileSize = this.files[0].size;
        $(this).parents(".file-upload").find(".file-name").text(fileName);
        if(fileSize < 1024 * 1024 * 3){
        readURL4(this);
        var ext = this.value.match(/\.(.+)$/)[1];
            switch (ext) {
                case 'jpg':
                case 'jpeg':
                case 'png':
                case 'gif':
                    break;
                default:
                    $(this).parents(".file-upload").find(".file-name").text('Not Allowed');
                    $(this).value = '';
            }
        }
        else{
            $(this).parents(".file-upload").find(".file-name").text('File Size Exceeding 3MB');
        }
    });
    
    /*========*/
     var maxHeight = 0;
    $('.dividing-panel .divide-unit').each(function(){
       maxHeight = $(this).height() > maxHeight ? $(this).height() : maxHeight;
    });
    $(".dividing-panel .unit-inner").css('min-height', maxHeight);
    
    
    /*=====================
        tabs
    =====================*/
    $("body").on("click",".tabs-custome a",function(e){
        var $this = $(this);
        e.preventDefault();
        $(this).parents(".tabs-custome").find("li").removeClass("active");
        $(this).parent().addClass("active");
        $(this).parents(".tabs-custome-wrapper").find(".tab-custome-pane").removeClass("active");
        $($(this).attr("href")+".tab-custome-pane").addClass("active");
        $($(this).attr("href")+".tab-custome-pane").addClass("opened");
        
    });
    
    $("body").on("click", ".accorTrigger", function (e) {
        var $this = $(this);
        e.preventDefault();
        $(this).parent().toggleClass("opened");
    });
    /*====Accordian====*/

    $("body").on("click", ".subAccorTrigger", function (e) {
        var $this = $(this);
        e.preventDefault();
        if ($(this).parent(".subAccorWrapper").hasClass("active")) {
            $(this).parent(".subAccorWrapper").removeClass("active");
        } else {
            $(".subAccorWrapper").removeClass("active");
            $(this).parent(".subAccorWrapper").addClass("active");
        }

    });
    /*====Modal====*/
    
    $('body').on("click", function (e) {
        if ($(e.target).closest('.modal-inner, .map-trigger, .addSubCommunityTrigger').length === 0) {
            $(".my-modal").fadeOut();
        }
    });
    
    $("body").on("click",".close-modal",function(e){
        var $this = $(this);
        e.preventDefault();
        $(this).parents(".my-modal").fadeOut();
    });
    
    $("body").on("click",".map-trigger",function(e){
        var $this = $(this);
        e.preventDefault();
        $(".my-modal.location-modal").fadeIn();
    });
    
    
    $("body").on("click", ".addSubCommunityTrigger", function (e) {
        var $this = $(this);
        e.preventDefault();
        $(".my-modal.sub-community-modal").fadeIn();
    });
    /*=====================
        scroll
    =====================*/
    
    $(".added-info-scroll").mCustomScrollbar();
    $(".added-communities-scroll").mCustomScrollbar();
    
    /*================
    Custom Select
    =================*/
    $('.custome-select select').on('change', function() {
        var p = $(this).parent(".custome-select");
        p.find('span').html($(this).find('option:selected').text());
    });
    
    var options = [];
    $('.dropdown-menu').on('click', function (event) {

        //var $target = $(event.currentTarget),
        //    val = $target.attr('data-value'),
        //    $inp = $target.find('input'),
        //    idx;

        //if ((idx = options.indexOf(val)) > -1) {
        //    options.splice(idx, 1);
        //    setTimeout(function () { $inp.prop('checked', false) }, 0);
        //} else {
        //    options.push(val);
        //    setTimeout(function () { $inp.prop('checked', true) }, 0);
        //}

        //$(event.target).blur();

        //console.log(options);
        event.stopPropagation();
    });

    
    /*================================
	Google Maps
	================================*/

    if ($('#contact-map').length) {
        var contact_map = 'contact-map',
            mapAddress = $('#contact-map').data('address'),
            mapType = $('#contact-map').data('maptype'),
            zoomLvl = $('#contact-map').data('zoomlvl');
        contactemaps(contact_map, mapAddress, mapType, zoomLvl);

    }

    function contactemaps(selector, address, type, zoom_lvl) {
        var map = new google.maps.Map(document.getElementById(selector), {
            mapTypeId: google.maps.MapTypeId.type,
            scrollwheel: false,
            draggable: false,
            zoom: zoom_lvl,
        });
        var map_pin = "assets/img/map-pin.png";
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({
                'address': address
            },
            function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    new google.maps.Marker({
                        position: results[0].geometry.location,
                        map: map,
                        icon: map_pin
                    });
                    map.setCenter(results[0].geometry.location);
                }
            });
    }
   
    







});