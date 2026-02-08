export class ApiHelper {
    dev = (isHttps) => {
        return isHttps ? 'https://localhost:700' : 'http://localhost:500';
    };
    base = (serviceName) => {
        return `http://${serviceName}:8080`;
    };
    productCatalogBaseAddress =   `${this.dev(false)}1/ProductCatalog`;
    shoppingCartBaseAddress =     `${this.dev(false)}2/ShoppingCart`;
    buyActionsBaseAddress =       `${this.dev(false)}3/BuyActions`;
    userManipulationBaseAddress = `${this.dev(false)}4/UserManipulations`;
}