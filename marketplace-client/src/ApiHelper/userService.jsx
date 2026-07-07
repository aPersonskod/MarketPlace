const userHost = import.meta.env.VITE_USER_APP_HOST;
const userPort = import.meta.env.VITE_USER_APP_PORT;

export const userServiceBaseAddress = `${userHost}:${userPort}/api/user-service`;

export const walletReplenishmentRequest = () => {
    try{
        let moneyDto = {
            money: 300
        };
        let query = `${userServiceBaseAddress}/wallet-replenishment?money=${money}`;
        const response = await fetch(query, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${getAccessToken()}`
            },
            body: JSON.stringify(moneyDto),
        });

        if (!response.ok) {
            let myLocalError = await response.json();
            throw new Error(`${myLocalError.error}`);
        }

        const data = await response.json();
        return data;
    } catch(e){
        console.error("Failed to top-up money:", e);
        throw e; 
    }
};

export const authRequest = async (email, password) => {
    try{
        let userCredentials = {
            email: formData.email,
            password: formData.password
        };
        const response = await fetch(`${userServiceBaseAddress}/login`, {
            method: 'POST',
            headers: {
                'accept': '*/*',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userCredentials),
        });
        if (!response.ok) {
            let localError = await response.json();
            throw new Error(`${myLocalError.error}`);
        }
        const result = await response.json();
        let token = result.token;
        localStorage.setItem('uToken', token);
    } catch(e){
        console.error("Failed to delete cart:", e);
        throw e; 
    }
};

export const getUser = async () => {
    let token = getAccessToken();
    if (token === null) return null;
    let url = `${userServiceBaseAddress}/`;
    let options = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
    };

    try{
        const response = await fetch(url, options);
        switch(response.status) {
            case 200:
                const userDto = await response.json();
                return userDto;

            case 401:
                console.log("401 Unauthorized - Authentication failed");
                //localStorage.removeItem('uToken');
                return null;
                
            case 403:
                console.log("403 Forbidden - Insufficient permissions");
                return null;
            default:
                console.log(`Unhandled HTTP status: ${response.status}`);
                return null;
        }
    }
    catch(e){
        console.log("Network or CORS error:", e);
        return null;
    }
}

export const getAccessToken = () => {
    return localStorage.getItem('uToken');
}

