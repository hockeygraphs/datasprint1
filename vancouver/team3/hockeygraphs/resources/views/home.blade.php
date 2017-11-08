@extends('layouts.app')

@section('content')
    <div class="legend">
        <span class="wasnt">Was Not Hit</span>
        <span class="was">Was Hit</span>
    </div>
    <div class="padding-left">
        <h1>Hitting is kind of weird</h1>
        
        <h2>If you get hit...</h2>

        <h3>Your shots per hour rate goes down</h3>
    </div>

    <div id="sph-viz" class="viz"></div>

    <h3>Your passes per hour rate goes down</h3>

    <div id="pph-viz" class="viz"></div>

    <h3>Your goals per hour rate goes down</h3>

    <div id="gph-viz" class="viz"></div>

    <h2>BUT...</h2>

    <h3>Strangely, your time of possession goes WAY UP</h3>
    <div id="pos-viz" class="viz"></div>
@endsection

@section('scripts')
    <script type="text/javascript">

        jQuery( document ).ready(function($) {

            var posData = [4.281, 6.53];
            var shotsData = [252.763, 107.294];
            var goalData = [4.505, 1.696];
            var passData = [868.129, 450.212];

            var divideValues = function(arr) {
                var largestNum = arr[0] > arr[1] ? arr[0] : arr[1];
                var percVal = arr.map(function(x) {
                    return x / largestNum;
                });
                return percVal * 100 + "px";
            }

            $('#sph-viz').on('click', function() {
                d3.select("#sph-viz")
                .selectAll("div")
                .data(shotsData)
                .enter().append("div")
                .attr("class", "bar")
                .style("width", function(d) { return (d / 252.763) * 500 + "px"; })
                .text(function(d) { return d; })
                .style('opacity', 0)
                .transition()
                .duration(1250)
                .style('opacity', 1);
            });

            $('#pph-viz').on('click', function() {
                d3.select("#pph-viz")
                .selectAll("div")
                .data(passData)
                .enter().append("div")
                .attr("class", "bar")
                .style("width", function(d) { return (d / 868.129) * 500 + "px"; })
                .text(function(d) { return d; })
                .style('opacity', 0)
                .transition()
                .duration(1250)
                .style('opacity', 1);
            });

            $('#gph-viz').on('click', function() {

                d3.select("#gph-viz")
                .selectAll("div")
                .data(goalData)
                .enter().append("div")
                .attr("class", "bar")
                .style("width", function(d) { return (d / 4.505) * 500 + "px"; })
                .text(function(d) { return d; })
                .style('opacity', 0)
                .transition()
                .duration(1250)
                .style('opacity', 1);
            });

            $('#pos-viz').on('click', function() {
                d3.select("#pos-viz")
                    .selectAll("div")
                    .data(posData)
                    .enter().append("div")
                    .attr("class", "bar")
                    .style("width", function(d) { return (d / 6.53) * 500 + "px"; })
                    .text(function(d) { return d; })                
                    .style('opacity', 0)
                    .transition()
                    .duration(1250)
                    .style('opacity', 1);
            });

        });
    </script>
@endsection
