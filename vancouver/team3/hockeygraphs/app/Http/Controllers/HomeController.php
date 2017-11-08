<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Collection;
use Excel;

class HomeController extends Controller
{
    /**
     * Show the application dashboard.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        $filepath = public_path() . '/hitData.csv';
        $hitData = Excel::load($filepath, function($reader) {})->get();
        $sorted = $hitData->sortByDesc('time')->slice(0,10);
        $hitsData = array();
        foreach($sorted as $key => $value) {
            array_push($hitsData, $value->time);
        }
        return view('home', compact('sorted', 'hitsData'));
    }
}

