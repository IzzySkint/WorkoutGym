export class AdminService {
    constructor() {
        this.endpoint = "/api/admin/";
    }
    getMemberSessionsByDate(date) {
        let url = this.endpoint + "getMemberSessionsByDate";
        url += "?date=" + date.toJSON();
        return new Promise((fulfill, reject) => {
            $.get(url).then((response) => {
                fulfill(response);
            }).fail((error) => {
                reject(error);
            });
        });
    }
    getMemberSessionsByDateRange(startDate, endDate) {
        let url = this.endpoint + "getMemberSessionsByDateRange";
        url += "?startDate=" + startDate.toJSON();
        url += "&endDate=" + endDate.toJSON();
        return new Promise((fulfill, reject) => {
            $.get(url).then((response) => {
                fulfill(response);
            }).fail((error) => {
                reject(error);
            });
        });
    }
}
//# sourceMappingURL=AdminService.js.map