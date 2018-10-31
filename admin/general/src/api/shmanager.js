import fetch from '@/utils/fetch'

export function BusinessCommList(method, data) {
    return fetch({
        url: 'BusinessCommList',
        method: method,
        params: data
    })
}
export function ShBankCard(method, data) {
    return fetch({
        url: 'ShBankCard',
        method: method,
        params: data
    })
}
export function EditShBankCard(method, data) {
    return fetch({
        url: 'ShBankCard',
        method: method,
        data
    })
}

export function QrCode(method, data) {
    return fetch({
        url: 'QrCode',
        method: method,
        params: data
    })
}
export function EditQrCode(method, data) {
    return fetch({
        url: 'QrCode',
        method: method,
        data
    })
}
export function BusinessrelayAmountList(method, data) {
    return fetch({
        url: 'BusinessrelayAmountList',
        method: method,
        params: data
    })
}
export function AuditBusinessrelayAmount(method, data) {
    return fetch({
        url: 'AuditBusinessrelayAmount',
        method: method,
        params: data
    })
}
export function BusinessrelayAmount(method, data) {
    return fetch({
        url: 'BusinessrelayAmount',
        method: method,
        params: data
    })
}
