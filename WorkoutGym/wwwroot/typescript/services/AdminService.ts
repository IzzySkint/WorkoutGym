import IAdminService = WorkoutGym.IAdminService;
import IMemberSessionModel = WorkoutGym.IMemberSessionModel;

export class AdminService implements IAdminService{

    private readonly endpoint = "/api/admin/";
    
    constructor() {
        
    }
    
    getMemberSessionsByDate(date: Date): Promise<IMemberSessionModel[]> {
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

    getMemberSessionsByDateRange(startDate: Date, endDate: Date): Promise<IMemberSessionModel[]> {
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