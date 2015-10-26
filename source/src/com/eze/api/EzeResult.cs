using System.Collections.Generic;

namespace com.eze.api 
{
public class EzeResult 
{

    private EventName eventName;
	private Status status;
        private Error error;
	private List<Result> resultList;
    private Result result;
   
   public List<Result> getResultList() {
		return resultList;
	}
	public void setResultList(List<Result> resultList) {
		this.resultList = resultList;
	}
        public Result getResult()
        {
            return result;
        }
        public void setResult(Result result)
        {
            this.result = result;
        }
        public EventName getEventName()
    {
            
        return eventName;
	}
    public void setEventName(EventName eventName)
    {
        this.eventName = eventName;
	}
	public Status getStatus() {
		return status;
	}
	public void setStatus(Status status) {
		this.status = status;
	}

        public Error getError()
        {
            return error;
        }
        public void setError(Error error)
        {
            this.error = error;
        }

        public override string ToString() {
        return "EzeResult [eventName=" + eventName + ", status=" +status+" ]";
	}
}

   
}