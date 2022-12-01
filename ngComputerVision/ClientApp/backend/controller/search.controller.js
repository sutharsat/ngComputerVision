const express = require('express');
const mongoose = require('mongoose');
const app = express();

const { Search } = require('../models/Search');
var SearchModel = mongoose.model('Search');

// Search
exports.search = (req, res, next) => {
  //ProductModel.find({
  //  person: { $regex: '.*' + req.body.person + '.*' }
  //},
  //      (error, data) => {
  //          if (error) {
  //              return next(error)
  //          } else {
  //              res.json(data)
  //          }
  //      }).limit(50);
  SearchModel.find({
    $or: [{ person: { $regex: '.*' + req.body.person + '.*' } },
      { email: { $regex: '.*' + req.body.person + '.*' } },
      { phoneNumber: { $regex: '.*' + req.body.person + '.*' } },
      { dateTime: { $regex: '.*' + req.body.person + '.*' } }]
  },
        (error, data) => {
            if (error) {
                return next(error)
            } else {
                res.json(data)
            }
        }).limit(50);
    
   
}













