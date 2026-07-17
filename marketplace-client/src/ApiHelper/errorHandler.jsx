export const throwHandledException = async (response) => {
    if (!response.ok) {
        let status = `status code ${response.status}:`;
        switch(response.status) {
            case 401:
                throw new Error(`${status} Authentication failed`);
                break;
            case 403:
                throw new Error(`${status} Insufficient permissions`);
                break;
            default:
                let myLocalError = await response.json();
                throw new Error(`${status} ${myLocalError.detail}`);
        }
    }
};