import fetch from '@/utils/external_fetch'

export function operationReissue(method, data) {
    return fetch({
        url: 'k_pay/pay/operationReissue.doAdminJJ',
        method: method,
        params: data
    })
}