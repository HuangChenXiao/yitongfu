
let PayMethod = value => {
    var valMap = {
        1: 'X',
        2: 'S',
    }
    return valMap[value]
}

export { PayMethod }