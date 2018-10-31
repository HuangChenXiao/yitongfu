import fetch from '@/utils/external_fetch'

export function merchBaseDataRegister(method, data) {
    return fetch({
        url: 'MLMerchant/merchBaseDataRegister/merchSet.doAdminJJ',
        method: method,
        params: data
    })
}

export function merchOpenBusiness(method, data) {
    return fetch({
        url: 'MLMerchant/merchOpenBusiness/getOpenBusiness.doAdminJJ',
        method: method,
        params: data
    })
}

export function merchBankCardRegister(method, data) {
    return fetch({
        url: 'MLMerchant/merchBankCardRegister/getBankCard.doAdminJJ',
        method: method,
        params: data
    })
}

export function getKey(method, data) {
    return fetch({
        url: 'XingYe/user/getKey.doAdminJJ',
        method: method,
        params: data
    })
}

export function withdrawPayAPI(method, data) {
    return fetch({
        url: 'k_pay/pay/withdrawPayAPI.doAdminJJ',
        method: method,
        params: data
    })
}