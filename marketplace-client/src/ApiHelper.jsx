export class ApiHelper {
    dev = (isHttps) => {
        return isHttps ? 'https://localhost:700' : 'http://localhost:500';
    };
    base = (serviceName) => {
        return `http://${serviceName}:8080`;
    };
    productCatalogBaseAddress =   `${this.dev(true)}1/ProductCatalog`;
    shoppingCartBaseAddress =     `${this.dev(true)}2/ShoppingCart`;
    buyActionsBaseAddress =       `${this.dev(true)}3/BuyActions`;
    userManipulationBaseAddress = `${this.dev(true)}4/UserManipulations`;

    getUser = async () => {
        let token = this.getAccessToken();
        if (token === null) return null;
        let url = `${this.userManipulationBaseAddress}`;
        let options = {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
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

    getAccessToken = () => {
        return localStorage.getItem('uToken');
    }
}