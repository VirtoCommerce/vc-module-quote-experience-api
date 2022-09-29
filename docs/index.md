## Overview

The X-Quote module provides queries and mutations for working with quotes via GraphQL.

## Endpoints
### Queries

|#|Endpoint|Arguments|Returns|Description|
|-|-|-|-|
|1|[quote](#quote)|`id`|Quote|Get quote by ID|
|2|[quotes](#quotes)|`customerId` `keyword` `sort` `after` `first`|Paginated quote list|Search for quotes|

### Mutations

|#|Endpoint|Arguments|Returns|Description|
|-|-|-|-|
|1|[createQuoteFromCart](#createQuoteFromCart)|`cartId` `comment`|Quote|Create quote and delete cart|
|2|[confirmQuote](#confirmQuote)|`id`|Order|Confirm quote and create order|
|3|[rejectQuote](#rejectQuote)|`id`|Quote|Reject quote|

## Examples
### quote
```
query {
  quote(id: "70e6807d-bd42-4c78-bc0d-bb2f3ff7ae65") {
    id
    number
    status
    comment
    totals {
      grandTotalInclTax {
        formattedAmount
      }
    }
    items {
      name
      sku
      selectedTierPrice {
        quantity
        price {
          formattedAmount
        }
      }
    }
  }
}
```

### quotes
```
query {
  quotes(
    customerId: "3e639def-722b-43d7-a77d-2e571dd39b87"
    sort: "createdDate"
    after: 0
    first: 10
  ) {
    totalCount
    items {
      id
      number
      status
      totals {
        grandTotalInclTax {
          formattedAmount
        }
      }
    }
  }
}
```

### createQuoteFromCart
```
mutation {
  createQuoteFromCart(command: { cartId: "a1f40b00-85ff-4328-9395-c1d5e28cdb8f", comment: "Some comment" }) {
    id
    number
    status
    comment
  }
}
```

### confirmQuote
```
mutation {
  confirmQuote(command: { id: "70e6807d-bd42-4c78-bc0d-bb2f3ff7ae65" }) {
    id
    number
    status
    comment
  }
}
```

### rejectQuote
```
mutation {
  rejectQuote(command: { id: "70e6807d-bd42-4c78-bc0d-bb2f3ff7ae65" }) {
    id
    number
    status
    comment
  }
}
```
