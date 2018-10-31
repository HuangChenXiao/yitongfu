import fetch from '@/utils/fetch'

export function agent(method, data) {
    return fetch({
        url: 'Agent',
        method: method,
        params: data
    })
}
export function editagent(method, data) {
    return fetch({
        url: 'Agent',
        method: method,
        data
    })
}

export function merchant(method, data) {
    return fetch({
        url: 'Merchant',
        method: method,
        params: data
    })
}
export function editmerchant(method, data) {
    return fetch({
        url: 'Merchant',
        method: method,
        data
    })
}

export function businessbasic(method, data) {
    return fetch({
        url: 'Businessbasic',
        method: method,
        params: data
    })
}
export function editbusinessbasic(method, data) {
    return fetch({
        url: 'Businessbasic',
        method: method,
        data
    })
}

export function merchantbankcard(method, data) {
    return fetch({
        url: 'MerchantBankCard',
        method: method,
        params: data
    })
}
export function editmerchantbankcard(method, data) {
    return fetch({
        url: 'MerchantBankCard',
        method: method,
        data
    })
}

export function openpayment(method, data) {
    return fetch({
        url: 'OpenPayment',
        method: method,
        params: data
    })
}
export function editopenpayment(method, data) {
    return fetch({
        url: 'OpenPayment',
        method: method,
        data
    })
}
export function GetNotOpenPayment(method) {
    return fetch({
        url: 'GetNotOpenPayment',
        method: "get",
    })
}
export function GetPlatTrade(method, data) {
    return fetch({
        url: 'GetPlatTrade',
        method: method,
        params: data
    })
}

export function Platbase(method) {
    return fetch({
        url: 'Platbase',
        method: "get",
    })
}
export function EditPlatbase(method, data) {
    return fetch({
        url: 'Platbase',
        method: method,
        data
    })
}
export function Merchantbusiness(method, data) {
    return fetch({
        url: 'Merchantbusiness',
        method: method,
        params: data
    })
}

export function setCashid(method, data) {
    return fetch({
        url: 'setCashid',
        method: method,
        data
    })
}

export function GetBusids(method, data) {
    return fetch({
        url: 'GetBusids',
        method: method,
        params: data
    })
}
export function BusinessPass(method, data) {
    return fetch({
        url: 'BusinessPass',
        method: method,
        params: data
    })
}
export function BusBalance(method, data) {
    return fetch({
        url: 'BusBalance',
        method: method,
        params: data
    })
}

export function AgentRatio(method, data) {
    return fetch({
        url: 'AgentRatio',
        method: method,
        params: data
    })
}
export function EditAgentRatio(method, data) {
    return fetch({
        url: 'AgentRatio',
        method: method,
        data
    })
}

export function SHBusinessAppinfoList(method, data) {
    return fetch({
        url: 'SHBusinessAppinfoList',
        method: method,
        params: data
    })
}
export function EditSHBusinessAppinfoList(method, data) {
    return fetch({
        url: 'SHBusinessAppinfoList',
        method: method,
        data
    })
}
export function UnionBalance(method, data) {
    return fetch({
        url: 'UnionBalance',
        method: method,
        params: data
    })
}
