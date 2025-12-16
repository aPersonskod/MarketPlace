export class ApiHelper {
    dev = (isHttps) => {
        return isHttps ? 'https://localhost:7' : 'http://localhost:5';
    };
    base = (serviceName) => {
        return `http://${serviceName}:8080`;
    };
    productCatalogBaseAddress =   `https://localhost:5001/ProductCatalog`;
    shoppingCartBaseAddress =     `${this.dev(false)}002/ShoppingCart`;
    buyActionsBaseAddress =       `${this.dev(false)}003/BuyActions`;
    userManipulationBaseAddress = `${this.dev(false)}004/UserManipulations`;
}