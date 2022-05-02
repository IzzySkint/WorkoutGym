import IMemberService = WorkoutGym.IMemberService;
import IMemberSessionBookingModel = WorkoutGym.IMemberSessionBookingModel;
import IMemberSessionModel = WorkoutGym.IMemberSessionModel;
import IWorkoutAreaSessionModel = WorkoutGym.IWorkoutAreaSessionModel;
import IWorkoutAreaModel = WorkoutGym.IWorkoutAreaModel;
import IBookingValidityCheckResultModel = WorkoutGym.IBookingValidityCheckResultModel;

export class MemberService implements IMemberService{
    
    private readonly endpoint = "/api/member/";
    
    constructor() {
        
    }
    
    createMemberSessionBooking(booking: IMemberSessionBookingModel): Promise<IMemberSessionModel> {
        let url = this.endpoint + "createMemberSessionBooking";

        return new Promise((fulfill, reject) => {
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(booking)
            }).then((response) => {
                fulfill(response);
            }).fail((jqXHR, textStatus, error) => {
                reject(error);
            });
                
        });
    }

    getMemberSessionsByDate(userId: string, date: Date): Promise<IMemberSessionModel[]> {
        let url = this.endpoint + "getMemberSessionsByDate";
        url += "?userId=" + userId;
        url += "&date=" + date.toJSON();

        return new Promise((fulfill, reject) => {
            $.get(url).then((response) => {
                fulfill(response);
            }).fail((error) => {
               reject(error); 
            });
        });
    }

    getMemberSessionsByDateRange(userId: string, startDate: Date, endDate: Date): Promise<IMemberSessionModel[]> {
        let url = this.endpoint + "getMemberSessionsByDateRange";
        url += "?userId=" + userId;
        url += "&startDate=" + startDate.toJSON();
        url += "&endDate=" + endDate.toJSON();

        return new Promise((fulfill, reject) => {
            $.get(url).then((response) => {
                fulfill(response);
            }).fail((error) => {
                reject(error);
            });
        });
    }

    getWorkoutAreaSessions(workoutAreaId: number, date: Date): Promise<IWorkoutAreaSessionModel[]> {
        let url = this.endpoint + "getWorkoutAreaSessions";
        url += "?workoutAreaId=" + workoutAreaId;
        url += "&date=" + date.toJSON();

        return new Promise((fulfill, reject) =>{
            $.get(url).then((response) => {
                fulfill(response);
            }).fail((jqXHR, textStatus, error) => {
                reject(error);
            });
        });
    }

    getWorkoutAreas(): Promise<IWorkoutAreaModel[]> {

        let url = this.endpoint + "getWorkoutAreas";

        return new Promise((fulfill, reject) =>{
            $.get(url)
                .then((response) => {
                    fulfill(response);
                })
                .fail((jqXHR, textStatus, error) => {
                    reject(error);
                });
        });
    }

    isMemberSessionBookingValid(booking: IMemberSessionBookingModel): Promise<IBookingValidityCheckResultModel> {
        let url = this.endpoint + "isMemberSessionBookingValid";
    
        return new Promise((fulfill, reject) => {
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(booking)
            }).then((response) => {
                fulfill(response);
            }).fail((jqXHR, textStatus, error) => {
                reject(error);
            });
        });
    }
}


