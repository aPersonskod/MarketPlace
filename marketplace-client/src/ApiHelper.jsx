export class ApiHelper {
    dev = (isDev) => {
        return isDev ? 'https' : 'http';
    };
    productCatalogBaseAddress =   `${this.dev(false)}://localhost:7001/ProductCatalog`;
    shoppingCartBaseAddress =     `${this.dev(false)}://localhost:7002/ShoppingCart`;
    buyActionsBaseAddress =       `${this.dev(false)}://localhost:7003/BuyActions`;
    userManipulationBaseAddress = `${this.dev(false)}://localhost:7004/UserManipulations`;
}