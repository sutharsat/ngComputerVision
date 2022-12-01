const express = require('express');
const app = express();
const searchRoute = express.Router();


let search = require('../controller/search.controller');

searchRoute.post('/search/search', search.search);



module.exports = searchRoute;
