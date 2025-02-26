# GraphQL Samples

## Queries

### Simple Query

```
query {
  heroes {
    totalCount
    items {
      age
      description
      id
      name
    }
  }
}
```

### Sorting by Name

```
query {
  heroes(order: { name: ASC }) {
    totalCount
    items {
      age
      description
      id
      name
    }
  }
}
```

### Sorting by Name then by Age

```
query {
  heroes(order: { name: ASC, age: DESC }) {
    totalCount
    items {
      age
      description
      id
      name
      abilities {
        id
        name
      }
    }
  }
}
```

### OffsetPaging

```
query {
  heroes(skip: 5, take: 5) {
    totalCount
    items {
      age
      description
      id
      name
      abilities {
        id
        name
      }
    }
    pageInfo {
      hasNextPage
      hasPreviousPage
    }
  }
}
```

## Mutations

### Add ability

```
mutation {
  addAbility(input: { abilityName: "TestAbility1" }) {
    uuid
  }
}
```

### Remove Ability

```
mutation {
  removeAbility(input: { abilityId: "860fa49a-6804-4cb2-8da7-1897bc173d5f" }) {
    boolean
  }
}
```
