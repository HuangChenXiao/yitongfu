import fetch from '@/utils/fetch'

export function Mercomm(method, data) {
    return fetch({
        url: 'Mercomm',
        method: method,
        params: data
    })
}
export function EditMercomm(method, data) {
    return fetch({
        url: 'Mercomm',
        method: method,
        params: data
    })
}