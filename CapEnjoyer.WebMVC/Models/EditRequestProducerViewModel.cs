namespace Cap.Enjoyer.WebMVC.Models;

public class EditRequestProducerViewModel
{
    public required int RequestCount { get; set; }
    public required ProducerDetailViewModel OriginalProducer { get; set; }
    public required ProducerDetailViewModel EditRequestProducer { get; set; }
}
