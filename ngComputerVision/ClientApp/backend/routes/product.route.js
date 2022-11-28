const express = require('express');
const app = express();
const productRoute = express.Router();

// Employee model 
// let Product = require('../models/Product');
let product = require('../controller/product.controller');

productRoute.post('/product/search', product.search);

//productRoute.post('/product', product.create);

//productRoute.get('/product', product.getAllProduct);

//productRoute.get('/getproduct/:pid', product.getProductById);

//productRoute.put('/update/:id', product.updateProduct);

//productRoute.delete('/delete/:id', product.deleteProduct);

module.exports = productRoute;
