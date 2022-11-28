const express = require('express');
const mongoose = require('mongoose');
const app = express();
// const employeeRoute = express.Router();

// Product model
// let Product = require('../models/Product');
const { Product } = require('../models/Product');
var ProductModel = mongoose.model('Product');

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
  ProductModel.find({
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
    // ProductModel.aggregate([
    //   {
    //     $search: {
    //       "autocomplete": {
    //         "path": "name",
    //         "query": req.body.name,
    //         "tokenOrder": "any"
    //       }
    //     }
    //   },
    //   {
    //     $limit: 15
    //   },
    //   {
    //     $project: {
    //       "_id": 0,
    //       "name": 1
    //     }
    //   }
    // ], function (error, data) {
    //   if (error) {
    //     return next(error)
    //   } else {
    //     res.json(data)
    //   }
    // })
}

// Add Product
//exports.create = (req, res, next) => {
//    if (req.body._id) {
//        updatePord(req, res, next);
//    } else {

//        var product = new ProductModel(req.body);
//        // save model to database
//        product.save(function(error, data) {
//            if (error) {
//                return next(error)
//            } else {
//                res.json(data)
//            }
//        });
//    }



//};

// Get All Products
//exports.getAllProduct = (req, res) => {
//    ProductModel.find((error, data) => {
//        if (error) {
//            return next(error)
//        } else {
//            res.json(data)
//        }
//    })
//}

// Get single Product
//exports.getProductById = (req, res) => {
//    ProductModel.findById(req.params.pid, (error, data) => {
//        if (error) {
//            return next(error)
//        } else {
//            res.json(data)
//        }
//    })
//};


// Update Product
//exports.updateProduct = (req, res, next) => {
//    ProductModel.findByIdAndUpdate(req.params.id, {
//        $set: req.body
//    }, (error, data) => {
//        if (error) {
//            return next(error);
//            console.log(error)
//        } else {
//            res.json(data)
//            console.log('Data updated successfully')
//        }
//    })
//}

// Delete Product
//exports.deleteProduct = (req, res, next) => {
//    ProductModel.findOneAndRemove(req.params.id, (error, data) => {
//        if (error) {
//            return next(error);
//        } else {
//            res.status(200).json({
//                msg: data
//            })
//        }
//    })
//}

//function updatePord(req, res, next) {
//    console.log(req.body);
//    ProductModel.findByIdAndUpdate(req.body._id, {
//        $set: req.body
//    }, (error, data) => {
//        if (error) {
//            console.log(error)
//            return next(error);

//        } else {
//            res.json(data)
//            console.log('Data updated successfully')
//        }
//    })
//}
