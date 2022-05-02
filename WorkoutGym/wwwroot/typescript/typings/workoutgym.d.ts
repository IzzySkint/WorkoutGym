declare namespace WorkoutGym{

    interface IWorkoutAreaSessionModel{
        areaId: number;
        sessionId: number;
        display: string;
        enabled: boolean;
    }

    interface IWorkoutAreaModel{
        id: number;
        name: string;
    }
    
    interface IMemberSessionBookingModel{
        userId: string;
        date: Date;
        workoutAreaId: number;
        workoutSessionId: number;
    }
    
    interface IMemberSessionModel{
        sessionId: number;
        member: string;
        email: string;
        date: string;
        startTime: string;
        endTime: string;
        area: string;
    }
    
    interface IBookingValidityCheckResultModel
    {
        isValid: boolean;
        message: string;
    }
    
    interface IMemberService{
        getWorkoutAreas(): Promise<IWorkoutAreaModel[]>;
        getWorkoutAreaSessions(workoutAreaId:number, date:Date): Promise<IWorkoutAreaSessionModel[]>;
        createMemberSessionBooking(booking: IMemberSessionBookingModel): Promise<IMemberSessionModel>;
        getMemberSessionsByDate(userId: string, date: Date): Promise<IMemberSessionModel[]>;
        getMemberSessionsByDateRange(userId: string, startDate: Date, endDate: Date): Promise<IMemberSessionModel[]>;
        isMemberSessionBookingValid(booking: IMemberSessionBookingModel): Promise<IBookingValidityCheckResultModel>;
    }
    
    interface IAdminService{
        getMemberSessionsByDate(date: Date): Promise<IMemberSessionModel[]>;
        getMemberSessionsByDateRange(startDate: Date, endDate: Date): Promise<IMemberSessionModel[]>;
    }
}