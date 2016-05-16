using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace _1031_Calculator
{
    public class AboutData
    {
        public AboutData() { }

        public AboutData(string question, string answer)
        {
            this.Question = question;
            this.Answer = answer;
        }

        public string Question { get; set; }
        public string Answer { get; set; }

        public static List<AboutData> GetFaqs()
        {
            List<AboutData> data = new List<AboutData>();
            string question = "What are the benefits of a Section 1031 tax deferred exchange?";
            string answer = "1031 tax deferred exchanges allow real estate investors to defer capital gain taxes on the sale of a property held for productive use in trade or business or for investment.  This tax savings provides many benefits including the obvious – 100% preservation of equity. Investors can take advantage of exchanges to meet other objectives including: A ) Leverage: exchanging from a high equity position or “free and clear” property into a much larger property with some financing in order to increase their return on investment.  B) Diversification: Such as exchanging into other geographical regions or diversifying by property type such as exchanging from several residential units into a retail strip center.  C) Management Relief: for example, exchanging out of multiple relinquished properties into either a replacement property like an apartment complex with an on-site manager or a tenant-in-common ownership program.";
            data.Add(new AboutData(question, answer));

            question = "What types of property can be exchanged and what is the definition of like-kind property?";
            answer = "Any property held for productive use of trade or business or for investment can be exchanged for any other property held for productive use in trade or business or for investment – these properties are considered “like-kind” to one another.  Examples of like-kind investment real estate include: exchanging unimproved for improved property; a fee interest for a leasehold with 30 or more years left; exchanging vacant raw land for a commercial building; or exchanging a single family rental for a small apartment complex.  The rules for exchanges of personal property are significantly more narrow and class or asset code specific than for real property.";
            data.Add(new AboutData(question, answer));

            question = "What should I look for when selecting a “Qualified Intermediary”?";
            answer = "There are a number of factors that should be considered before selecting a Qualified Intermediary, sometimes referred to as a “QI” to facilitate your exchange.  One of the most important is researching specific security measures they provide for the exchange proceeds held in their possession during the exchange period.  This is crucial because there is no entity at the Federal level that oversees or regulates Qualified Intermediaries and how the proceeds are invested and kept safe for investors.  In addition, find out the experience of the company, how many exchanges and the knowledge level of their staff and diversity of experience.";
            data.Add(new AboutData(question, answer));

            question = "What are the requirements for full tax deferral?";
            answer = "For full tax deferral, an exchanger must reinvest all of the new proceeds in “like-kind” replacement property and have the same or greater amount of debt on the replacement property or properties.  Another way of looking at this is to purchase replacement property of equal or greater value, and reinvest all of the net equity.";
            data.Add(new AboutData(question, answer));

            question = "What is a delayed exchange and what time limits are involved?";
            answer = "A delayed exchange happens when the exchanger closes on the sale of their relinquished property on one date, and then acquires a replacement property from a seller at a later date.  An exchanger has a maximum of 180 calendar days, or their tax filing date, whichever is earlier, to complete their exchange.  This is called the “exchange period”.  In addition, the exchanger must identify their potential replacement property or properties by midnight of the 45th day after closing on the sale of their relinquished property.  This is called the “identification period” and the 45 days are inclusive within the 180 day exchange period.";
            data.Add(new AboutData(question, answer));

            question = "Why should I exchange rather than sell?";
            answer = "A 1031 exchange allows an investor to defer paying all of the capital gain taxes, which essentially equates to a long-term and interest-free loan from the Internal Revenue Service.  Since the investor who exchanges has all of their gross net equity available to reinvest, using leverage, they can acquire significantly more investment real estate that if they sold, paid all the capital gain taxes and then purchased a new a property.  The real advantage of a 1031 exchange is not just the tax savings – it is the tremendous increase in purchasing power generated by this tax savings!";
            data.Add(new AboutData(question, answer));

            return data;
        }
    }
}