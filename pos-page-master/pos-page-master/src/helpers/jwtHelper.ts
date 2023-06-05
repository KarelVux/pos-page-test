// https://stackoverflow.com/questions/38552003/how-to-decode-jwt-token-in-javascript-without-using-a-library
function parseJwt(token: string): string {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

export function findUserNameFromJwt(token: string): string | undefined {
    return findDataByEndingKey(parseJwt(token), "name");
}

export function findUserRoleFromJwt(token: string): string | undefined {
    return findDataByEndingKey(parseJwt(token), "role");
}

function findDataByEndingKey(data: any, ending: string): string | undefined {
    for (const key in data) {
        if (key.endsWith(ending)) {
            return data[key];
        }
    }
    return undefined;
}