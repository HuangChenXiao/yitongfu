import fetch from '@/utils/external_fetch'


export function createorder(method, data) { //
    return fetch({
        url: 'yt_pay/pay/createorder.do',
        method: method,
        params: data
    })
}



