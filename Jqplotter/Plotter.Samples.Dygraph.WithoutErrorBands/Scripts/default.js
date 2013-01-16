//
// Copyright 2012 Akram El Assas.
//
// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements. See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership. The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied. See the License for the
// specific language governing permissions and limitations
// under the License.


function pageLoad() {

    // colorPicker
    $('#TextBoxColor,#TextBoxRangeSelectorPlotStrokeColor,#TextBoxRangeSelectorPlotFillColor').ColorPicker({
        onSubmit: function(hsb, hex, rgb, el) {
            $(el).val(hex);
            $(el).ColorPickerHide();
        },
        onBeforeShow: function() {
            $(this).ColorPickerSetColor(this.value);
        }
    }).bind('keyup', function() {
        $(this).ColorPickerSetColor(this.value);
    });

    // Larger thumbnail preview 
    // hoverIntent
    // This effect is done by @SohTanaka, an Interactive Designer & Front-end developer from Los Angeles.
    $("ul.thumb li").hover(function() {
        $(this).css({ 'z-index': '10' });
        $(this).find('img').addClass('hover').stop()
            .animate({
                marginTop: '-110px',
                marginLeft: '-110px',
                top: '50%',
                left: '50%',
                width: '174px',
                height: '174px',
                padding: '20px'
            }, 200);

    }, function() {
        $(this).css({ 'z-index': '0' });
        $(this).find('img').removeClass('hover').stop()
            .animate({
                marginTop: '0',
                marginLeft: '0',
                top: '0',
                left: '0',
                width: '100px',
                height: '100px',
                padding: '5px'
            }, 400);
    });
}