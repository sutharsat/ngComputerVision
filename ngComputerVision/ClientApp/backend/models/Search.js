const mongoose = require('mongoose');
const Schema = mongoose.Schema;

// Define collection and schema
let Search = new Schema({
  person: { type: String },
  organization: { type: String },
  address: { type: String },
  phoneNumber: { type: String },
  email: { type: String },
  dateTime: { type: String },
  claimId: { type: String },
  //Image: {type: String}
}, {
    collection: 'Search'
})


module.exports = mongoose.model('Search', Search)
